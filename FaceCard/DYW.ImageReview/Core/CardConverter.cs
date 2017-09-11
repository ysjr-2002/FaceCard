using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace DYW.ImageReview.Core
{
    class CardConverter
    {
        public static void PrintBitArray(BitArray array)
        {
            for (var i = array.Length - 1; i >= 0; i--)
            {
                var item = array[i];
                var b = item ? 1 : 0;
                Console.Write(b + "");
            }
            Console.Write('\n');
        }

        public static void PrintAB(BitArray array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var item = array[i];
                var b = item ? 1 : 0;
                Console.Write(b + "");
            }
            Console.Write('\n');
        }

        public static BitArray BitArrayRevserve(BitArray array)
        {
            BitArray reverse = new BitArray(array.Length);
            var j = 0;
            for (var i = array.Length - 1; i >= 0; i--)
            {
                var item = array[i];
                reverse[j] = item;
                j++;
            }
            return reverse;
        }


        public static int GetCardNo(byte[] cardBuffer)
        {
            BitArray array1 = new BitArray(new byte[] { cardBuffer[0] });
            BitArray array2 = new BitArray(new byte[] { cardBuffer[1] });
            BitArray array3 = new BitArray(new byte[] { cardBuffer[2] });
            BitArray array4 = new BitArray(new byte[] { cardBuffer[3] });
            BitArray array5 = new BitArray(new byte[] { cardBuffer[4] });

            array1 = BitArrayRevserve(array1);
            array2 = BitArrayRevserve(array2);
            array3 = BitArrayRevserve(array3);
            array4 = BitArrayRevserve(array4);
            array5 = BitArrayRevserve(array5);

            var nb1 = array1.BitArrayToByte();
            var nb2 = array2.BitArrayToByte();
            var nb3 = array3.BitArrayToByte();
            var nb4 = array4.BitArrayToByte();
            var nb5 = array5.BitArrayToByte();

            BitArray cardTotal = new BitArray(new byte[] { nb1, nb2, nb3, nb4, nb5 });
            BitArray card = new BitArray(20);
            var j = 0;
            for (var i = 15; i < cardTotal.Length; i++)
            {
                card[j] = cardTotal[i];
                Console.Write((card[j] ? "1" : "0"));
                j++;

                if (j == 20)
                    break;
            }

            card = BitArrayRevserve(card);
            var cardNo = card.BitArrayToInt();

            return cardNo;
        }
    }
}
