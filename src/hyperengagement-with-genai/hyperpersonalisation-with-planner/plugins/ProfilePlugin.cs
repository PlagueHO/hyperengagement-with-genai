using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace hyperpersonalisation_with_planner.plugins;

internal sealed class CustomerPlugin
{
    /// <summary>
    /// Plugin to get the customers profile information
    /// </summary>
    /// <param name="emailAddress">The email address of the customer whose profile should be returned</param>
    /// <param name="logger">The logger</param>
    /// <returns>Current date and time in UTC format</returns>
    [KernelFunction, Description("Given the Contoso books customer e-mail address, return their profile information including name, age, location and list of interests")]
    public string GetCustomerProfile(
        [Description("The email address of the customer whose profile should be returned.")] string emailAddress,
        ILogger? logger = null)
    {
        logger?.LogInformation("GetCustomerProfile: returning the customer profile for {0}", emailAddress);

        // In a real application this would be looked up using an API or database request
        return @"
            Name: Jane Smith
            Age: 35
            Location: Manly, Sydney, Australia
            Interests (based on recent purchases and searches): DevOps, Horror Fiction, Fantasy, Software Development, Travel";
    }

    /// <summary>
    /// Plugin to get the customers recent purchases from Contoso Books
    /// </summary>
    /// <param name="emailAddress">The email address of the customer whose recent purchases should be returned</param>
    /// <param name="logger">The logger</param>
    /// <returns>Current date and time in UTC format</returns>
    [KernelFunction, Description("Given the Contoso books customer e-mail address, return a CSV containing the customers recent purchases from Contoso Books")]
    public string GetCustomerRecentPurchases(
        [Description("The email address of the customer whose recent purchases should be returned.")] string emailAddress,
        ILogger? logger = null)
    {
        logger?.LogInformation("GetCustomerRecentPurchases: returning the customer recent purchases for {0}", emailAddress);

        // In a real application this would be looked up using an API or database request
        return @"
Book name, Author, Publisher, Topics, Description, Purchase date  
""IT"", Stephen King, Viking Press, Horror Fiction, ""The story follows the experiences of seven children as they are terrorized by an evil entity that exploits the fears of its victims to disguise itself while hunting its prey."", 2020-02-14  
""The Stand"", Stephen King, Doubleday, Horror Fiction, ""A post-apocalyptic horror/fantasy novel that outlines the total breakdown and destruction of society after the release of a virulent man-made biological weapon."", 2020-03-20  
""Salem's Lot"", Stephen King, Doubleday, Horror Fiction, ""A novel about a small town that is invaded by vampires."", 2020-06-10  
""The Shining"", Stephen King, Doubleday, Horror Fiction, ""The story of a man's gradual descent into madness through the influences of the supernatural."", 2020-08-05  
""Doctor Sleep"", Stephen King, Scribner, Horror Fiction, ""The sequel to The Shining, featuring an adult Danny Torrance as he tries to protect a young girl with similar powers from a cult."", 2020-11-17  
""Continuous Delivery"", Jez Humble, Addison-Wesley, DevOps, ""A revolutionary and scalable agile methodology to deliver robust software as rapidly as possible."", 2021-01-25  
""Lean Enterprise"", Jez Humble, O'Reilly Media, DevOps, ""A guide to transforming your organization into a lean enterprise."", 2021-03-14  
""The Dark Tower: The Gunslinger"", Stephen King, Grant, Fantasy, ""The first book in The Dark Tower series, introducing the protagonist, Roland Deschain."", 2021-06-07  
""The Dark Tower: The Drawing of the Three"", Stephen King, Grant, Fantasy, ""The second book in The Dark Tower series, where Roland Deschain draws three people from our world into his."", 2021-07-22  
""The Dark Tower: The Waste Lands"", Stephen King, Grant, Fantasy, ""The third book in The Dark Tower series, as Roland Deschain and his ka-tet move forward to the Dark Tower."", 2021-11-08  
""Clean Code: A Handbook of Agile Software Craftsmanship"", Robert C. Martin, Prentice Hall, Software Development, ""A guide to writing code that is easy to read, understand, and maintain."", 2022-01-15  
""Refactoring: Improving the Design of Existing Code"", Martin Fowler, Addison-Wesley, Software Development, ""A guide to restructuring code in a disciplined way."", 2022-03-10  
""The Dark Tower: Wizard and Glass"", Stephen King, Grant, Fantasy, ""The fourth book in The Dark Tower series, tells Roland Deschain's origin story."", 2022-06-05  
""Lonely Planet's Ultimate Travel: Our List of the 500 Best Places to See"", Lonely Planet, Lonely Planet, Travel, ""A comprehensive list of the world's best travel destinations."", 2022-08-20  
";
    }
}
