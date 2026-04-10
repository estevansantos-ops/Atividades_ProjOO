using System;
using System.Collections.Generic;


// SINGLETON

public class ConfigGlobal
{
    private static ConfigGlobal _instance;

    public string NomeAplicacao { get; set; }
    public string ServidorEnvio { get; set; }
    public int Tentativas { get; set; }

    // Construtor privado para impedir o uso de 'new' fora da classe
    private ConfigGlobal()
    {
        NomeAplicacao = "sistema inicial";
        ServidorEnvio = "unifesp.com";
        Tentativas = 3;
    }

    public static ConfigGlobal GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ConfigGlobal();
        }
        return _instance;
    }
}


// INTERFACE COMUM

public interface INotificacao
{
    void Enviar(string mensagem);
}


// IMPLEMENTAÇÕES (Factory)

public class EmailNotificacao : INotificacao
{
    public void Enviar(string mensagem)
    {
        string servidor = ConfigGlobal.GetInstance().ServidorEnvio;
        Console.WriteLine($"[Email] enviado via {servidor}: {mensagem}");
    }
}

public class SmsNotificacao : INotificacao
{
    public void Enviar(string mensagem)
    {
        Console.WriteLine($"[SMS] enviado: {mensagem}");
    }
}

public class PushNotificacao : INotificacao
{
    public void Enviar(string mensagem)
    {
        string app = ConfigGlobal.GetInstance().NomeAplicacao;
        Console.WriteLine($"[Push] para '{app}': {mensagem}");
    }
}


// FACTORY

public class ComunicacaoFactory
{
    public static INotificacao CriarComunicacao(string tipo)
    {
        if (string.IsNullOrEmpty(tipo)) return null;

        switch (tipo.ToUpper())
        {
            case "EMAIL":
                return new EmailNotificacao();
            case "SMS":
                return new SmsNotificacao();
            case "PUSH":
                return new PushNotificacao();
            default:
                throw new ArgumentException("Tipo de notificação desconhecido.");
        }
    }
}

// ADAPTER — API externa de SMS com interface incompatível

// Classe legada/externa que NÃO implementa INotificacao.
// Tem seus próprios métodos com assinatura diferente.
public class ApiExternaSms
{
    public void SendSms(string numero, string texto)
    {
        Console.WriteLine($"[API Externa] SMS para {numero}: {texto}");
    }
}

// Adapter que faz a ponte entre ApiExternaSms e INotificacao
public class ApiExternaSmsAdapter : INotificacao
{
    private readonly ApiExternaSms _apiExterna;
    private readonly string _numeroDestino;

    public ApiExternaSmsAdapter(ApiExternaSms apiExterna, string numeroDestino)
    {
        _apiExterna = apiExterna;
        _numeroDestino = numeroDestino;
    }

    // Traduz a chamada Enviar() para o método incompatível SendSms()
    public void Enviar(string mensagem)
    {
        _apiExterna.SendSms(_numeroDestino, mensagem);
    }
}


// PROXY — Controle de acesso, logs e limite de tentativas

public class NotificacaoProxy : INotificacao
{
    private readonly INotificacao _notificacaoReal;
    private readonly string _usuario;
    private readonly List<string> _usuariosPermitidos;
    private int _tentativasRealizadas;

    public NotificacaoProxy(INotificacao notificacaoReal, string usuario, List<string> usuariosPermitidos)
    {
        _notificacaoReal = notificacaoReal;
        _usuario = usuario;
        _usuariosPermitidos = usuariosPermitidos;
        _tentativasRealizadas = 0;
    }

    public void Enviar(string mensagem)
    {
        int limite = ConfigGlobal.GetInstance().Tentativas;

        // Validação de permissão
        if (!_usuariosPermitidos.Contains(_usuario))
        {
            Console.WriteLine($"[Proxy][BLOQUEADO] Usuário '{_usuario}' não tem permissão para enviar.");
            return;
        }

        // Controle de limite de tentativas
        if (_tentativasRealizadas >= limite)
        {
            Console.WriteLine($"[Proxy][BLOQUEADO] Limite de {limite} envios atingido para '{_usuario}'.");
            return;
        }

        // Log antes do envio
        _tentativasRealizadas++;
        Console.WriteLine($"[Proxy][LOG] Envio #{_tentativasRealizadas} por '{_usuario}'");

        // Delega ao objeto real
        _notificacaoReal.Enviar(mensagem);
    }
}