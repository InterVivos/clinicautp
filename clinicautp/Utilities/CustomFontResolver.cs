 using PdfSharpCore.Fonts;
 using Android.Content;
 
 public class CustomFontResolver : IFontResolver
    {
        public string DefaultFontName => throw new NotImplementedException();

        public byte[] GetFont(string faceName)
        {
            var task = FileSystem.OpenAppPackageFileAsync(faceName);
            var stream = task.Result;
            byte[] bytes;
            List<byte> totalStream = new();
            byte[] buffer = new byte[32];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                totalStream.AddRange(buffer.Take(read));
            }
            bytes = totalStream.ToArray();
            return bytes;
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if(familyName.Equals("OpenSans", StringComparison.CurrentCultureIgnoreCase))
            {
                return new FontResolverInfo("OpenSans-Regular.ttf");
            }

            return PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
        }
    }