using Application.DTO.Order;
using FluentAssertions;

namespace ApplicationTest.IntegrationTests.Order;

[TestFixture]
public class OrderFilesTests : OrderTest
{
    [Test]
    public async Task PutNewPhotos()
    {
        var id = await PostValidOrder();
        var fileNames = new[] {"photo1.jpg", "photo2.png"};
        var response = await PutPhotos(id, fileNames);

        response.IsSuccessStatusCode.Should().BeTrue();
        
        var getResponse = await Client.GetAsync($"{BaseUrl}/{id}");
        var order = await ExtractFromResponse<OrderDetailsVm>(getResponse);
        
        order.Should().NotBeNull();
        order.Photos.Should().HaveCount(2);
        order.Photos
            .Zip(fileNames, 
                (orderPhoto, fileName) => orderPhoto.Contains(fileName))
            .All(x => x)
            .Should().BeTrue();
        
        await FileService.DeleteFiles(CancellationToken.None, order.Photos);
    }

    private async Task<HttpResponseMessage> PutPhotos(Guid id, params string[] fileNames)
    {
        using var content = new MultipartFormDataContent();
        foreach (var fileName in fileNames)
        {
            var fileContent = new ByteArrayContent(new byte[] { 0x1, 0x2, 0x3, 0x4 });
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            // Важно: "Photos" — это имя свойства в вашем PhotoDto (или имя параметра в контроллере)
            content.Add(fileContent, "Photos", fileName);
        }
        var response = await Client.PutAsync($"{BaseUrl}/{id}/photos", content);
        return response;
    }
}