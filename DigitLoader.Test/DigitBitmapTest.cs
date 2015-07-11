﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitLoader.Test
{
    [TestClass]
    public class DigitBitmapTest
    {
        // Sample Record
        string sample = "3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,21,130,190,254,254,250,175,135,96,96,16,4,0,0,0,0,0,0,0,0,0,0,0,0,0,26,102,186,254,254,248,222,222,225,254,254,254,254,254,206,112,4,0,0,0,0,0,0,0,0,0,0,0,207,254,254,177,117,39,0,0,56,248,102,48,48,103,192,254,135,0,0,0,0,0,0,0,0,0,0,0,91,111,36,0,0,0,0,0,72,92,0,0,0,0,12,224,210,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,50,139,240,254,66,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,121,220,254,244,194,15,0,0,0,0,0,0,0,0,0,0,0,0,0,8,107,112,112,112,87,112,141,218,248,177,68,20,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,77,221,254,254,254,254,254,225,104,39,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,10,32,32,32,32,130,215,195,47,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,111,231,174,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,47,18,0,0,0,0,0,0,0,0,0,40,228,205,35,0,0,0,0,0,0,0,0,0,0,0,0,22,234,42,0,0,0,0,0,0,0,0,0,0,56,212,226,38,0,0,0,0,0,0,0,0,0,0,0,96,157,0,0,0,0,0,0,0,0,0,0,0,0,30,215,188,9,0,0,0,0,0,0,0,0,0,0,96,142,0,0,0,0,0,0,0,0,0,0,0,0,0,86,254,68,0,0,0,0,0,0,0,0,0,0,71,202,15,0,0,0,0,0,0,0,0,0,0,0,0,6,214,151,0,0,0,0,0,0,0,0,0,0,10,231,86,2,0,0,0,0,0,0,0,0,0,0,0,0,191,207,0,0,0,0,0,0,0,0,0,0,0,93,248,129,7,0,0,0,0,0,0,0,0,0,0,117,238,112,0,0,0,0,0,0,0,0,0,0,0,0,94,248,209,73,12,0,0,0,0,0,0,42,147,252,136,9,0,0,0,0,0,0,0,0,0,0,0,0,0,48,160,215,230,158,74,64,94,153,223,250,214,105,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,11,129,189,234,224,255,194,134,75,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";

        int[] array0 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array1 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array2 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array3 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array4 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array5 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array6 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 21, 130, 190, 254, 254, 250, 175, 135, 96, 96, 16, 4, 0, 0, 0, 0, 0, 0, 0 };
        int[] array7 = new int[28] { 0, 0, 0, 0, 0, 0, 26, 102, 186, 254, 254, 248, 222, 222, 225, 254, 254, 254, 254, 254, 206, 112, 4, 0, 0, 0, 0, 0 };
        int[] array8 = new int[28] { 0, 0, 0, 0, 0, 0, 207, 254, 254, 177, 117, 39, 0, 0, 56, 248, 102, 48, 48, 103, 192, 254, 135, 0, 0, 0, 0, 0 };
        int[] array9 = new int[28] { 0, 0, 0, 0, 0, 0, 91, 111, 36, 0, 0, 0, 0, 0, 72, 92, 0, 0, 0, 0, 12, 224, 210, 5, 0, 0, 0, 0 };
        int[] array10 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 139, 240, 254, 66, 0, 0, 0, 0, 0 };
        int[] array11 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 121, 220, 254, 244, 194, 15, 0, 0, 0, 0, 0, 0 };
        int[] array12 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 8, 107, 112, 112, 112, 87, 112, 141, 218, 248, 177, 68, 20, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array13 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 77, 221, 254, 254, 254, 254, 254, 225, 104, 39, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array14 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 10, 32, 32, 32, 32, 130, 215, 195, 47, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array15 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 111, 231, 174, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array16 = new int[28] { 0, 0, 0, 0, 0, 47, 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 228, 205, 35, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array17 = new int[28] { 0, 0, 0, 0, 22, 234, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 56, 212, 226, 38, 0, 0, 0, 0, 0, 0, 0 };
        int[] array18 = new int[28] { 0, 0, 0, 0, 96, 157, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 215, 188, 9, 0, 0, 0, 0, 0, 0 };
        int[] array19 = new int[28] { 0, 0, 0, 0, 96, 142, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 86, 254, 68, 0, 0, 0, 0, 0, 0 };
        int[] array20 = new int[28] { 0, 0, 0, 0, 71, 202, 15, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 6, 214, 151, 0, 0, 0, 0, 0, 0 };
        int[] array21 = new int[28] { 0, 0, 0, 0, 10, 231, 86, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 191, 207, 0, 0, 0, 0, 0, 0 };
        int[] array22 = new int[28] { 0, 0, 0, 0, 0, 93, 248, 129, 7, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 117, 238, 112, 0, 0, 0, 0, 0, 0 };
        int[] array23 = new int[28] { 0, 0, 0, 0, 0, 0, 94, 248, 209, 73, 12, 0, 0, 0, 0, 0, 0, 42, 147, 252, 136, 9, 0, 0, 0, 0, 0, 0 };
        int[] array24 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 48, 160, 215, 230, 158, 74, 64, 94, 153, 223, 250, 214, 105, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array25 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 11, 129, 189, 234, 224, 255, 194, 134, 75, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array26 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] array27 = new int[28] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        [TestMethod]
        public void OutputArray_OnGenerateDigitArray_First10Match()
        {
            // Arrange / Act
            var output = DigitBitmap.GenerateDigitArray(sample);

            // Assert
            for (int i = 0; i < 28; i++)
            {
                Assert.AreEqual(array0[i], output[0, i]);
                Assert.AreEqual(array1[i], output[1, i]);
                Assert.AreEqual(array2[i], output[2, i]);
                Assert.AreEqual(array3[i], output[3, i]);
                Assert.AreEqual(array4[i], output[4, i]);
                Assert.AreEqual(array5[i], output[5, i]);
                Assert.AreEqual(array6[i], output[6, i]);
                Assert.AreEqual(array7[i], output[7, i]);
                Assert.AreEqual(array8[i], output[8, i]);
                Assert.AreEqual(array9[i], output[9, i]);
            }
        }

        [TestMethod]
        public void OutputArray_OnGenerateDigitArray_Middle10Match()
        {
            // Arrange / Act
            var output = DigitBitmap.GenerateDigitArray(sample);

            // Assert
            for (int i = 0; i < 28; i++)
            {
                Assert.AreEqual(array10[i], output[10, i]);
                Assert.AreEqual(array11[i], output[11, i]);
                Assert.AreEqual(array12[i], output[12, i]);
                Assert.AreEqual(array13[i], output[13, i]);
                Assert.AreEqual(array14[i], output[14, i]);
                Assert.AreEqual(array15[i], output[15, i]);
                Assert.AreEqual(array16[i], output[16, i]);
                Assert.AreEqual(array17[i], output[17, i]);
                Assert.AreEqual(array18[i], output[18, i]);
                Assert.AreEqual(array19[i], output[19, i]);
            }
        }

        [TestMethod]
        public void OutputArray_OnGenerateDigitArray_Last8Match()
        {
            // Arrange / Act
            var output = DigitBitmap.GenerateDigitArray(sample);

            // Assert
            for (int i = 0; i < 28; i++)
            {
                Assert.AreEqual(array20[i], output[20, i]);
                Assert.AreEqual(array21[i], output[21, i]);
                Assert.AreEqual(array22[i], output[22, i]);
                Assert.AreEqual(array23[i], output[23, i]);
                Assert.AreEqual(array24[i], output[24, i]);
                Assert.AreEqual(array25[i], output[25, i]);
                Assert.AreEqual(array26[i], output[26, i]);
                Assert.AreEqual(array27[i], output[27, i]);
            }
        }
    }
}
