using System.Collections.Generic;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("-> Teste Singleton <-\n");
        TesteSingleton();

        Console.WriteLine("\n-> Teste Factory <-\n");
        TesteFactory();

        Console.WriteLine("\n-> Teste Adapter <-\n");
        TesteAdapter();

        Console.WriteLine("\n-> Teste Proxy <-\n");
        TesteProxy();
    }

    public static void TesteSingleton()
    {
        ConfigGlobal config1 = ConfigGlobal.GetInstance();
        ConfigGlobal config2 = ConfigGlobal.GetInstance();

        config1.NomeAplicacao = "exercicio_pratico";

        if (Object.ReferenceEquals(config1, config2))
            Console.WriteLine("config1 e config2 são a mesma instância na memória.");
        else
            Console.WriteLine("as instâncias são diferentes.");

        if (config2.NomeAplicacao == "exercicio_pratico")
            Console.WriteLine("A alteração feita em config1 foi identificada em config2.");
        else
            Console.WriteLine("Os dados não estão sincronizados.");
    }

    public static void TesteFactory()
    {
        INotificacao email = ComunicacaoFactory.CriarComunicacao("EMAIL");
        INotificacao sms = ComunicacaoFactory.CriarComunicacao("SMS");
        INotificacao push = ComunicacaoFactory.CriarComunicacao("PUSH");

        if (email is EmailNotificacao) Console.WriteLine("Objeto Email criado.");
        if (sms is SmsNotificacao) Console.WriteLine("Objeto SMS criado.");
        if (push is PushNotificacao) Console.WriteLine("Objeto Push criado.");

        Console.WriteLine("\n> teste de envio:");
        email.Enviar("Bem-vindo!");
        sms.Enviar("Código: 1234");
        push.Enviar("Nova atualização");

        Console.WriteLine("\n> teste tipo inválido:");
        try
        {
            ComunicacaoFactory.CriarComunicacao("INVALIDO");
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Factory bloqueou tipo desconhecido corretamente.");
        }
    }

    public static void TesteAdapter()
    {
        // API externa com interface incompatível (SendSms ao invés de Enviar)
        ApiExternaSms apiLegada = new ApiExternaSms();

        // Adapter permite usar a API externa como INotificacao
        INotificacao smsAdaptado = new ApiExternaSmsAdapter(apiLegada, "+55-11-99999-0000");

        // Agora funciona com a mesma interface do sistema
        smsAdaptado.Enviar("Seu código de verificação é 5678");

        // Pode ser usado no lugar de qualquer INotificacao
        Console.WriteLine("\n> adapter usado de forma transparente:");
        var canais = new List<INotificacao>
        {
            ComunicacaoFactory.CriarComunicacao("EMAIL"),
            smsAdaptado,
            ComunicacaoFactory.CriarComunicacao("PUSH")
        };
        foreach (var canal in canais)
        {
            canal.Enviar("Mensagem para todos os canais");
        }
    }

    public static void TesteProxy()
    {
        var permitidos = new List<string> { "admin", "operador" };

        // Proxy sobre um Email real
        INotificacao emailReal = ComunicacaoFactory.CriarComunicacao("EMAIL");

        Console.WriteLine("> usuário autorizado (admin):");
        INotificacao proxyAdmin = new NotificacaoProxy(emailReal, "admin", permitidos);
        proxyAdmin.Enviar("Relatório mensal");
        proxyAdmin.Enviar("Alerta de segurança");

        Console.WriteLine("\n> usuário não autorizado (hacker):");
        INotificacao proxyHacker = new NotificacaoProxy(emailReal, "hacker", permitidos);
        proxyHacker.Enviar("Tentativa de acesso");

        // Teste de limite de tentativas (limite = 3 do Singleton)
        Console.WriteLine("\n> teste de limite de envios (limite=3):");
        INotificacao smsReal = ComunicacaoFactory.CriarComunicacao("SMS");
        INotificacao proxyOperador = new NotificacaoProxy(smsReal, "operador", permitidos);
        proxyOperador.Enviar("Mensagem 1");
        proxyOperador.Enviar("Mensagem 2");
        proxyOperador.Enviar("Mensagem 3");
        proxyOperador.Enviar("Mensagem 4 - deve ser bloqueada");
    }
}