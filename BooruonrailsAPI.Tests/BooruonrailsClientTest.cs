using System;
using NUnit.Framework;
using BooruonrailsAPI;
using BooruonrailsAPI.Enums;
using BooruonrailsAPI.Exceptions;
using BooruonrailsAPI.Responses;
using System.Diagnostics;

namespace BooruonrailsAPI.Tests
{
    [TestFixture]
    public class BooruonrailsClientTest
    {
        private const string APIKey_Expected = "b1vdApr3qruoFKDdKDzy";
        private const string username = "techmail.techmail@mail.ru";
        private const string password = "BooruonrailsAPItech";
        private const string baseUri = "http://0s.mrsxe4djmjxw64tvfzxxezy.nblz.ru/";//"https://trixiebooru.org";

        [Test]
        public void GetImages([Values(SearchMethod.AllTop, SearchMethod.Favourites, SearchMethod.Images, SearchMethod.Search, SearchMethod.Top, SearchMethod.WatchList)]SearchMethod method)
        {
            const int expectedLenght = 5;

            BooruonrailsClient client = new BooruonrailsClient(baseUri);
            client.SignIn(username, password);
            BooruonrailsImage[] images = client.GetImages(method, "perpage=" + Convert.ToString(expectedLenght), "sbq=safe");

            Console.WriteLine("Found {0} images by search method named \"{1}\"", images.Length, method.ToString());
            foreach (BooruonrailsImage img in images)
            {
                Console.WriteLine("Image #{0}\r\n\tScore: {1}\r\n\tInteractions count: {2}", img.ID, img.Score, img.Interactions.Length);
                foreach (BooruonrailsInteraction inter in img.Interactions)
                    Console.WriteLine("\t\t{0} - {1}", inter.InteractionType, inter.Value);
            }

            Assert.AreEqual(expectedLenght, images.Length);
        }

        [Test]
        public void GetImage([Values(702641)] int idNumber)
        {
            BooruonrailsClient client = new BooruonrailsClient(baseUri);
            client.SignIn(username,password);
            BooruonrailsImage img = client.GetImage(idNumber);

            Console.WriteLine("Found image by search method named \"{0}\"", "GetImage()");
            Console.WriteLine("Image #{0}\r\n\tScore: {1}\r\n\tInteractions count: {2}", img.ID, img.Score, img.Interactions.Length);
            foreach (BooruonrailsInteraction inter in img.Interactions)
                Console.WriteLine("\t\t{0} - {1}", inter.InteractionType, inter.Value);

            Assert.AreEqual(idNumber, Convert.ToInt32(img.ID));
        }

        [Test]
        public void GetAPIKey()
        {
            BooruonrailsClient client = new BooruonrailsClient(baseUri);

            Console.Write("Authorization by {0}... ", username);
            client.SignIn(username, password);
            Console.WriteLine("success");
            string APIKey_Current = client.GetAPIKey();
            Console.WriteLine("API Key: {0}", APIKey_Current);

            Assert.AreEqual(APIKey_Expected, APIKey_Current);
        }

        [Test]
        public void CallInteractionMethod([Values(InteractionMethod.Vote, InteractionMethod.Favourites)] InteractionMethod Method, [Values(858027)] int idNumber)
        {
            BooruonrailsClient client = new BooruonrailsClient(baseUri);
            BooruonrailsImage img = client.GetImage(idNumber, "key=" + APIKey_Expected);

            string argruement_value = "value=";

            foreach (BooruonrailsInteraction i in img.Interactions)
                if ((i.InteractionType == "faved" && Method == InteractionMethod.Favourites) || (i.InteractionType == "voted" && Method == InteractionMethod.Vote))
                    argruement_value += "false";

            if (argruement_value == "value=")
                if (InteractionMethod.Vote == Method)
                    argruement_value += "down";
                else if (InteractionMethod.Favourites == Method)
                    argruement_value += "true";

            BooruonrailsRating updatedRating = client.CallInteractionMethod(Method,argruement_value, "key=" + APIKey_Expected, "id=" + img.ID);
            Assert.IsNotNull(updatedRating);
        }

        [Test]
        public void GetTags([Values(TagsType.Aliases, TagsType.All, TagsType.Implied)]TagsType type)
        {
            BooruonrailsClient client = new BooruonrailsClient(baseUri);
            BooruonrailsTag[] tags = client.GetTags(type);

            Console.WriteLine("Found {0} tags by type named \"{1}\"", tags.Length, type.ToString());
            foreach (BooruonrailsTag tag in tags)
                Console.WriteLine("Tag named \"{0}\"\r\n\tImages count: {1}\r\n\tDescription: {2}\r\n", tag.Name, tag.ImagesCount, tag.ShortDescription);

            Assert.AreNotEqual(0, tags.Length);
        }

        [Test]
        public void GetTag([Values("safe", "40482", "ThrowsExceptionTag")] string id)
        {
            BooruonrailsClient client = new BooruonrailsClient(baseUri);
            BooruonrailsTag tag = client.GetTag(id);
            if (id == "ThrowsExceptionTag" && tag == null)
                Assert.Pass();
            Console.WriteLine("Tag named \"{0}\"\r\n\tImages count: {1}\r\n\tDescription: {2}\r\n", tag.Name, tag.ImagesCount, tag.ShortDescription);
            Assert.Pass();
        }

        [Test]
        public void GetFilters([Values("fq=dick")] string parameter)
        {
            BooruonrailsClient client = new BooruonrailsClient(baseUri);
            BooruonrailsFilter[] filters = client.GetFilters(parameter);
            Assert.AreEqual("Wall of dicks",filters[filters.Length - 1].Name);
        }
    }
}
