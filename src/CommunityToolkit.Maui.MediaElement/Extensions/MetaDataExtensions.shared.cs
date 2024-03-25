using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Core;

namespace CommunityToolkit.Maui.Extensions;
/// <summary>
/// 
/// </summary>
public partial class MetaDataExtensions
{
	/// <summary>
	/// 
	/// </summary>
	public MetaDataExtensions()
	{
	}
}

class ID3Tag
{
	public string? Title { get; set; }
	public string? Album { get; set; }
	public string? Artist { get; set; }
	public byte[]? ImageData { get; set; } = [byte.MinValue];

	/// <summary>
	/// A class that provides methods to extract ID3 tags from an MP3 file.
	/// </summary>
	public ID3Tag()
	{
		// Get the image data from the MP3 file

	}

	/// <summary>
	/// A method that extracts the image data from an MP3 file.
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns></returns>
	public byte[] GetImageDataFromFile(string filePath)
	{
		// Check if the file exists
		if (!File.Exists(filePath))
		{
			return [];
		}

		using FileStream stream = File.OpenRead(filePath);
		// Extract ID3 tags
		TagLib.File file = TagLib.File.Create(new StreamFileAbstraction(filePath, stream));

		// Extract title, album, and artist
		Title = file.Tag.Title;
		Album = file.Tag.Album;
		Artist = file.Tag.FirstPerformer;

		// Extract binary image (if available)
		var pictures = file.Tag.Pictures;
		if (pictures.Length > 0)
		{
			var picture = pictures[0];
			return picture.Data.Data; // Retrieve the image data as a byte array
									  // Now you can use 'imageData' as needed, such as storing it in a variable or processing it further.

		}
		else
		{
			return [];
		}
	}

	/// <summary>
	/// A method that extracts the image data from a URL.
	/// </summary>
	/// <param name="url"></param>
	/// <returns></returns>
	public async Task<byte[]> GetImageDataFromUrl(string url)
	{
		using HttpClient client = new();
		// Download the MP4 file as a binary stream
		using Stream stream = await client.GetStreamAsync(url);
		// Extract ID3 tags
		TagLib.File file = TagLib.File.Create(new StreamFileAbstraction(url, stream));

		// Extract title, album, and artist
		Title = file.Tag.Title;
		Album = file.Tag.Album;
		Artist = file.Tag.FirstPerformer;

		// Extract binary image (if available)
		var pictures = file.Tag.Pictures;
		if (pictures.Length > 0)
		{
			var picture = pictures[0];
			return picture.Data.Data; // Retrieve the image data as a byte array
									  // Now you can use 'imageData' as needed, such as storing it in a variable or processing it further.
		}
		else
		{
			return [];
		}
	}
}

class StreamFileAbstraction : TagLib.File.IFileAbstraction, IDisposable
{
	readonly Stream stream;
	readonly Stream readWriteStream;

	/// <summary>
	/// The name of the file.
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// The stream containing the file data.
	/// </summary>
	/// <param name="name"></param>
	/// <param name="stream"></param>
	public StreamFileAbstraction(string name, Stream stream)
	{
		Name = name;
		this.stream = stream;

		// Create a read/write memory stream
		readWriteStream = new MemoryStream();
		stream.CopyTo(readWriteStream);
		readWriteStream.Seek(0, SeekOrigin.Begin); // Reset position to the beginning
	}

	/// <summary>
	/// The read stream of the file.
	/// </summary>
	public Stream ReadStream
	{
		get { return readWriteStream; }
	}

	/// <summary>
	/// The write stream of the file.
	/// </summary>
	public Stream? WriteStream
	{
		// Since MemoryStream doesn't support writing, return null
		get { return null; }
	}

	/// <summary>
	/// Closes the stream.
	/// </summary>
	/// <param name="stream"></param>
	public void CloseStream(Stream stream)
	{
		if (stream == this.stream)
		{
			// Dispose the original stream when done
			this.stream.Dispose();
		}
		else
		{
			// Dispose the read/write memory stream
			readWriteStream.Dispose();
		}
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			stream.Dispose();
			readWriteStream.Dispose();
		}
	}
}