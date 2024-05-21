//===============================================================================
// Copyright © Avanza Solutions (Pvt) Ltd.  All rights reserved.
// THIS CODE AND INFORMATION IS PROPERTY OF THE AVANZA SOLUTIONS AND 
// CANNOT BE USED WITHOUT THE APPROVAL OF THE MANAGEMENT
//===============================================================================

using System;
using System.IO;
using System.Globalization;

namespace Avanza.Core.Utility
{
    public static class IoUtil
    {
        public static string GetCompleteUrl(string fileName)
        {
            //appending date            
            fileName = GetFileName(fileName);           

            //Meer Salman @ 16-Mar-15 : Concatenating File Name & Commenting below line
            //if ((!Path.IsPathRooted(fileName)) && (fileName.IndexOf('\\') == -1))
            if ((!Path.IsPathRooted(fileName)) && (fileName.Split('\\').Length <= 4))
                fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            //Closing Tag : Concatenating File Name & Commenting below line

            return fileName;
        }
        
        public static string GetCompleteRelatedUrl(string fileName)
        {
            //appending date            
            fileName = GetFileName(fileName);

            if ((!Path.IsPathRooted(fileName)) && (fileName.IndexOf('\\') == -1))
                fileName = Path.Combine
                            (
                              (AppDomain.CurrentDomain.RelativeSearchPath == null ) ? AppDomain.CurrentDomain.BaseDirectory :AppDomain.CurrentDomain.RelativeSearchPath,
                              fileName
                            );

            return fileName;
        }

        public static string GetCompleteUrl(string fileName,bool dateChange)
        {
            string retVal = fileName;
            if (dateChange)
            {
                string completePath = fileName;
                if ((!Path.IsPathRooted(fileName)) && (fileName.IndexOf('\\') == -1))
                    completePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                string dirctoryPath = completePath.Substring(0, completePath.LastIndexOf('\\'));
                string fileNameOnly = completePath.Split('\\')[completePath.Split('\\').Length - 1];

                int startIndex = fileNameOnly.IndexOf('{');
                int lastIndex = fileNameOnly.LastIndexOf('}') + 1;
                if (startIndex == -1 || lastIndex == -1 || lastIndex < startIndex)
                    return IoUtil.GetCompleteUrl(fileName); //so that old implementation dont change
                string dateFormat = fileNameOnly.Substring(startIndex + 1, lastIndex - startIndex - 2);
                string searchVal = fileNameOnly.Substring(0, startIndex) + "*" + fileNameOnly.Substring(lastIndex, (fileNameOnly.Length - lastIndex));
                                
                DirectoryInfo di = new DirectoryInfo(dirctoryPath);
                FileInfo[] rgFiles = di.GetFiles(searchVal);
                if (rgFiles.Length == 0)
                    return dirctoryPath + "\\" + searchVal.Replace("*","DateVal"); //no file exist
                Nullable<DateTime> maxValue = null;
                Nullable<DateTime> dt = null;
                foreach(FileInfo fi in rgFiles)
                {
                    string tmp = fi.Name.Substring(startIndex, dateFormat.Length);
                    try
                    {
                        dt = DateTime.ParseExact(tmp, dateFormat, new System.Globalization.CultureInfo("en-US", true));
                    }
                    catch (Exception ex)
                    {
                        continue; 
                    }
                    if (maxValue == null || dt > maxValue)
                    {
                        maxValue = dt;
                        retVal = dirctoryPath + "\\" + fi.Name;
                    }
                }
            }
            else
            {
                retVal = IoUtil.GetCompleteUrl(fileName);
            }

            return retVal;
        }
        public static string GetFileName(string fileName)
        {
            string retVal = fileName;
            try
            {
                int startIndex = fileName.IndexOf('{');
                int lastIndex = fileName.LastIndexOf('}') + 1;
                if (startIndex == -1 || lastIndex == -1 || lastIndex < startIndex)
                    return fileName;
                string dateFormat = fileName.Substring(startIndex + 1, lastIndex - startIndex - 2);
                string dateNow = DateTime.Now.ToString(dateFormat);
                retVal = fileName.Substring(0, startIndex) + dateNow + fileName.Substring(lastIndex, (fileName.Length - lastIndex));
            }
            catch (Exception e)
            {
                return retVal; 
            }
            return retVal;
        }

    }
}
