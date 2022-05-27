// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

public interface IClient
{
    public string UserName { get; set; }
    public string UserAuthString { get; set; }
    public bool HasAccess { get; set; }
    public void BuildAuthString(string UserName);

}

public class User : IClient
{
    public string UserName { get; set; }
    public string UserAuthString { get; set; }

    public bool HasAccess { set; get; } = false;

    public User(string name)
    {
        UserName = name;

    }

    public void BuildAuthString(string UserName)
    {
        UserAuthString = UserName;
    }
}

public class Manager : IClient
{
    public string UserName { get; set; }
    public string UserAuthString { get; set; }

    public bool HasAccess { set; get; } = true;

    public Manager(string name)
    {
        UserName = name;
    }

    public void BuildAuthString(string UserName)
    {
        UserAuthString = UserName + "MAN";
    }
}

public class Admin : IClient
{
    public string UserName { get; set; }
    public string UserAuthString { get; set; }

    public bool HasAccess { set; get; } = true;

    public Admin(string name)
    {
        UserName = name;
    }

    public void BuildAuthString(string UserName)
    {
        UserAuthString = UserName + "ADMIN";
    }
}
//**********************************************************************************************************************************
public interface AccessBehaviour
{
    public IClient Client { get; set; }

    public bool HandleAccess(IClient Client);
}

public class CheckString : AccessBehaviour
{
    public IClient Client { set; get; }


    public bool HandleAccess(IClient user)
    {
        var userAccess = user.UserAuthString;
        bool result = false;
        if (userAccess.Substring(userAccess.Length - 5) == "ADMIN")
        {
            result = true;

        }
        else
        {
            result = false;
        }


        return result;
    }
}

public class SwichAuth : AccessBehaviour
{
    public IClient Client { set; get; }

    public bool HandleAccess(IClient user)
    {
        if (user.HasAccess)
        {
            user.HasAccess = false;
        }
        else if (!user.HasAccess)
        {
            user.HasAccess = true;
        }

        return user.HasAccess;
    }
}
//*******************************************************************************************************************************

public class ClientFactory
{
    public IClient CreateClient(string clientType, string userName)
    {
        IClient newClient = new User(userName);
        if (clientType == "Manager")
        {
            newClient = new Manager(userName);
        }
        else if (clientType == "Admin")
        {
            newClient = new Admin(userName);
        }

        newClient.BuildAuthString(userName);

        return newClient;
    }
}

public abstract class ClientHandler
{
    public ClientFactory ClientFactory { get; set; }

    public abstract IClient CreateClient(string clientType, string userName);
}

public class RetailClientHandler : ClientHandler
{
    public RetailClientHandler()
    {
        ClientFactory = new ClientFactory();
    }

    public override IClient CreateClient(string clientType, string userName)
    {
        return ClientFactory.CreateClient(clientType, userName);
    }

}

public class EnterpriseClientHandler : ClientHandler
{
    AccessBehaviour AccessBehaviour { get; set; }
    public EnterpriseClientHandler()
    {
        ClientFactory = new ClientFactory();
        AccessBehaviour = new CheckString();
    }

    public override IClient CreateClient(string clientType, string userName)
    {
        return ClientFactory.CreateClient(clientType, userName);
    }
}