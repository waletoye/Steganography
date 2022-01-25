using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using steganography.Models;

namespace steganography.Utilities
{
    class Steganography
    {
        public Bitmap ConcealText(string input, Bitmap image)
        {
            int R = 0, G = 0, B = 0;

            // current character index being concealed
            int charIndex = 0;

            // current character
            int charValue = 0;

            // current (R or G or B) element
            long rgbIndex = 0;

            // number of trailing zeros that have been added when finishing the process
            int trailingZeros = 0;


            State state = State.Concealing;

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    // holds the pixel that is currently being processed
                    Color pixel = image.GetPixel(j, i);

                    // now, clear the least significant bit (LSB) from each pixel element
                    R = pixel.R - pixel.R % 2;
                    G = pixel.G - pixel.G % 2;
                    B = pixel.B - pixel.B % 2;

                    for (int n = 0; n < 3; n++)
                    {
                        // process 1 byte each(8 bits)
                        if (rgbIndex % 8 == 0)
                        {
                            // if all 8 bits are replace with zeros, then process is coplete
                            if (state == State.Concealed && trailingZeros == 8)
                            {
                                // modify the last pixel on the image
                                if ((rgbIndex - 1) % 3 < 2)
                                {
                                    image.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }

                                // return the bitmap with the text concealed in
                                return image;
                            }

                            // check if all characters have been concealed
                            //else conceal next
                            if (charIndex >= input.Length)
                            {
                                state = State.Concealed;
                            }
                            else
                            {
                                charValue = input[charIndex++];
                            }
                        }

                        // check which pixel element has the turn to hide a bit in its LSB

                        if (rgbIndex % 3 == 0)
                        {
                            if (state == State.Concealing)
                            {
                                R += charValue % 2;
                                charValue /= 2;
                            }
                        }
                        else if (rgbIndex % 3 == 1)
                        {
                            if (state == State.Concealing)
                            {
                                G += charValue % 2;

                                charValue /= 2;
                            }
                        }

                        else if (rgbIndex % 3 == 2)
                        {
                            if (state == State.Concealing)
                            {
                                B += charValue % 2;

                                charValue /= 2;
                            }

                            image.SetPixel(j, i, Color.FromArgb(R, G, B));
                        }

                        rgbIndex++;

                        if (state == State.Concealed)
                        {
                            // increment the value of zeros until it is 8
                            trailingZeros++;
                        }
                    }
                }
            }

            return image;
        }

        public string RetrieveConcealedText(Bitmap bmp)
        {
            int colorUnitIndex = 0, charValue = 0;

            var retrievedText = new StringBuilder();


            // pass through the rows
            for (int i = 0; i < bmp.Height; i++)
            {
                // pass through each row
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);

                    // for each pixel, pass through its elements (RGB)
                    for (int n = 0; n < 3; n++)
                    {
                        if (colorUnitIndex % 3 == 0)
                        {
                            charValue = charValue * 2 + pixel.R % 2;
                        }
                        if (colorUnitIndex % 3 == 1)
                        {
                            charValue = charValue * 2 + pixel.G % 2;
                        }

                        if (colorUnitIndex % 3 == 2)
                        {
                            charValue = charValue * 2 + pixel.B % 2;
                        }

                        colorUnitIndex++;

                        // if 8 bits has been added, then add the current character to the result text
                        if (colorUnitIndex % 8 == 0)
                        {
                            charValue = BitManipulator.ReverseByte(charValue);

                           //if end of byte '\0'
                            if (charValue == 0)
                                return retrievedText.ToString();

                            // convert the character value from int to char
                            char c = (char)charValue;

                            // add the current character to the result text
                            retrievedText.Append(c.ToString());
                        }
                    }
                }
            }

            return retrievedText.ToString();
        }

    }
}
