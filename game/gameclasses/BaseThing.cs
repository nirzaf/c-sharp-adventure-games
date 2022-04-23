
using System;

[Serializable]
public class BaseThing {
    // The ultimate ancestor of all game objects. Actual objects will probably never
    // be created directly from it.
    // BaseThing defines name and description - Thing descends from it
    private string _name;
    private string _description;
   

    public BaseThing(string aName, string aDescription) {        
        _name = aName;
        _description = aDescription;        
    }

    public string Name   //  Name property
        {
        get => _name;
        set => _name = value;
    }

    public string Description   // Description property
    {
        get => _description;
        set => _description = value;
    }
   
}
