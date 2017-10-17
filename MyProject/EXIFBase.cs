using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace EXIFInform
{
    public class EXIFBase
    {
        /// <summary>
        /// Change/Add standard EXIF field vlaue
        /// </summary>
        /// <param name="file"></param>
        /// <param name="objExifInfo"></param>
        /// <returns></returns>
        public static bool ChangeImageExif(string file, EXIFInfo objExifInfo)
        {
            #region Vars
            string sPropValue = string.Empty;
            EXIFBase.ExifField field;
            PropertyItem propItem = null;
            ImageFormat ifOriginal = null;
            Graphics gSave = null;
            Image iOriginal = null;
            Image iSave = null;
            #endregion

            Console.WriteLine("Started");

            try
            {
                iOriginal = new Bitmap(file);
                ifOriginal = iOriginal.RawFormat;
                Console.WriteLine("Bitmapped file");

                // For each EXIFField in objExifInfo, add it to Image EXIF
                foreach (var exField in objExifInfo)
                {
                    field = (EXIFBase.ExifField)Enum.Parse(typeof(EXIFBase.ExifField), exField.Key.ToString());
                    try
                    {
                        // Get the EXIF value from Image
                        Console.WriteLine("Getting EXIF value from image");
                        propItem = iOriginal.GetPropertyItem((int)field);
                        sPropValue = System.Text.Encoding.UTF8.GetString(propItem.Value);

                        //Change the value
                        sPropValue = sPropValue.Replace(sPropValue, exField.Value);

                        // Get bytes
                        propItem.Value = System.Text.Encoding.UTF8.GetBytes(sPropValue);

                        //Set the property on the image
                        iOriginal.SetPropertyItem(propItem);
                    }
                    catch (System.ArgumentException)
                    {
                        Console.WriteLine("EXIF param does not exist, creating new");
                        // EXIF tag doesn't exist, add it to image
                        WriteEXIFField(iOriginal, field, exField.Value.ToString() + "\0");
                    }
                }
                //Store the list of properties that exist on the image
                ArrayList alPropertyItems = new ArrayList();

                foreach (PropertyItem pi in iOriginal.PropertyItems)
                    alPropertyItems.Add(pi);

                //Create temp image
                iSave = new Bitmap(iOriginal.Width, iOriginal.Height, iOriginal.PixelFormat);
                Console.WriteLine("Temp img created");
                //Copy the original image over to the temp image
                gSave = Graphics.FromImage(iSave);

                //If you check iSave at this point, it does not have any EXIF properties -
                //only the image gets recreated
                gSave.DrawImage(iOriginal, 0, 0, iOriginal.Width, iOriginal.Height);
                Console.WriteLine(iSave);
                //Get rid of the locks on the original image
                iOriginal.Dispose();
                gSave.Dispose();

                //Copy the original EXIF properties to the new image
                foreach (PropertyItem pi in alPropertyItems)
                    iSave.SetPropertyItem(pi);

                //Save the temp image over the original image
                Console.WriteLine(iSave);
                iSave.Save(file, ifOriginal);
                Console.WriteLine("Pavyko");


                return true;
            }
            catch (Exception)
            {
                // TODO: Exception logging
                return false;
            }
            finally
            {
                iSave.Dispose();
            }
        }

        /// <summary>
        /// Add a standard EXIF field to the image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="field"></param>
        /// <param name="fieldValue"></param>
        private static void WriteEXIFField(Image image, ExifField field, string fieldValue)
        {
            Encoding asciiEncoding = new ASCIIEncoding();
            System.Text.Encoder encoder = asciiEncoding.GetEncoder();
            char[] tagTextChars = fieldValue.ToCharArray();
            int byteCount = encoder.GetByteCount(tagTextChars, 0, tagTextChars.Length, true);
            byte[] tagTextBytes = new byte[byteCount];
            encoder.GetBytes(tagTextChars, 0, tagTextChars.Length, tagTextBytes, 0, true);

            if (image.PropertyItems != null && image.PropertyItems.Length > 0)
            {
                PropertyItem propertyItem = image.PropertyItems[0];
                propertyItem.Id = (int)field;
                propertyItem.Type = 2;  // ASCII
                propertyItem.Len = tagTextBytes.Length;
                propertyItem.Value = tagTextBytes;
                image.SetPropertyItem(propertyItem);
            }
        }
        public enum ExifField : int
        {
            Latitude = 0x0002,
            Longitude = 0x0004

            /// More fields here
        }
    }







    public class EXIFInfo : Dictionary<EXIFBase.ExifField, string>
    {
        /// <summary>
        /// Initializes a new instance of the EXIFInfo class.
        /// </summary>
        public EXIFInfo() : base()
        {
            // Default constructor
        }

        /// <summary>
        /// Adds an EXIFField with the specified property value into the EXIFInfo
        /// </summary>
        public new void Add(EXIFBase.ExifField key, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value can not be empty");

            base.Add(key, value);
        }




            public static void DoIt(string fileName)
            {
                EXIFInfo info = new EXIFInfo();

                /// Add Exif Info to be added/updated
                info.Add(EXIFBase.ExifField.Latitude, "54.675051");
                info.Add(EXIFBase.ExifField.Longitude, "25.273356");
            Console.WriteLine("Starting EXIF edit");

                /// Call the main function
                EXIFBase.ChangeImageExif(fileName, info);
            ExtractMetaData(fileName);
        }


        private static void ExtractMetaData(String file)
{
            String Long = "";
            String Lat = "";
            try
    {
        // Create an Image object. 
        Image theImage = new Bitmap(file);

        // Get the PropertyItems property from image.
        PropertyItem[] propItems = theImage.PropertyItems;

        // Set up the display.
        Font font1 = new Font("Arial", 10);
  

        // For each PropertyItem in the array, display the id, 
        // type, and length.
        int count = 0;
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                foreach ( PropertyItem propItem in propItems )
        {

                    Console.WriteLine("   ID: 0x" +
                        propItem.Id.ToString("x"));
                    Console.WriteLine("   type: " +
                propItem.Type.ToString());
                    Console.WriteLine("   length: " +
                        propItem.Len.ToString() +
                        " bytes");
                    
                    string manufacturer = encoding.GetString(propItem.Value);

                   Console.WriteLine(
                       "Value of metadata " + manufacturer + ".");
                    count += 1;
                    if (propItem.Id == 4)
                    {
                        Long = manufacturer;
                    }
                    else if (propItem.Id == 2)
                    {
                        Lat = manufacturer;
                    }
                    manufacturer = "";
        }
    }
    catch(Exception)
    {
                Console.WriteLine("There was an error." + 
            "Make sure the path to the image file is valid.");
    }
            Console.WriteLine("Latitude: " + Lat + "Longitude: " + Long);

}
        }

    }
