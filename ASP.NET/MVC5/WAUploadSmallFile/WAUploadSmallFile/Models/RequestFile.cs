using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WAUploadSmallFile.Models
{
	public class RequestFile
	{
		public string ContentType { get; set; }
		public string ContentDisposition { get; set; }
		//public IHeaderDictionary Headers { get; set; }
		public long Length { get; set; }
		public string Name { get; set; }
		public string FileName { get; set; }

		//public Stream OpenReadStream();
		//public void CopyTo(Stream target);
		//public Task CopyToAsync(Stream target, CancellationToken cancellationToken);
	}
}