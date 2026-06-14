using System.ComponentModel.DataAnnotations;

public class UsersTemplateCreate
{
    [Required(ErrorMessage = "Username is required.")]
    [MinLength(1, ErrorMessage = "Username cannot be empty.")]
    public string username {set; get;}

    [MinLength(1, ErrorMessage = "Name cannot be empty.")]
    public string? name {set; get;} = null;
    public string? description {set; get;} = null;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(1, ErrorMessage = "Password cannot be empty.")]
    public string password {set; get;}
    public string visibility {set; get;} = "a";
    public string language {set; get;} = "PT";
    public UsersTemplateEntryObject entriesProprieties {set; get;} = new UsersTemplateEntryObject();
} 