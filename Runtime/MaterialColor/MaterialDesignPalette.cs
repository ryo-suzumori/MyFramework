using UnityEngine;

namespace MyFw
{
    public enum MaterialColorKey
    {
        Red,
        Pink,
        Purple,
        DeepPurple,
        Indigo,
        Blue,
        LightBlue,
        Cyan,
        Teal,
        Green,
        LightGreen,
        Lime,
        Yellow,
        Amber,
        Orange,
        DeepOrange,
        Brown,
        Grey,
        BlueGrey
    }

    public enum MaterialColorWeight
    {
        _50 = 50,
        _100 = 100,
        _200 = 200,
        _300 = 300,
        _400 = 400,
        _500 = 500,
        _600 = 600,
        _700 = 700,
        _800 = 800,
        _900 = 900
    }

    public static class MaterialDesignPalette
    {
        public static Color GetColor(MaterialColorKey color, MaterialColorWeight weight)
        {
            return color switch
            {
                MaterialColorKey.Red => GetRedColor(weight),
                MaterialColorKey.Pink => GetPinkColor(weight),
                MaterialColorKey.Purple => GetPurpleColor(weight),
                MaterialColorKey.DeepPurple => GetDeepPurpleColor(weight),
                MaterialColorKey.Indigo => GetIndigoColor(weight),
                MaterialColorKey.Blue => GetBlueColor(weight),
                MaterialColorKey.LightBlue => GetLightBlueColor(weight),
                MaterialColorKey.Cyan => GetCyanColor(weight),
                MaterialColorKey.Teal => GetTealColor(weight),
                MaterialColorKey.Green => GetGreenColor(weight),
                MaterialColorKey.LightGreen => GetLightGreenColor(weight),
                MaterialColorKey.Lime => GetLimeColor(weight),
                MaterialColorKey.Yellow => GetYellowColor(weight),
                MaterialColorKey.Amber => GetAmberColor(weight),
                MaterialColorKey.Orange => GetOrangeColor(weight),
                MaterialColorKey.DeepOrange => GetDeepOrangeColor(weight),
                MaterialColorKey.Brown => GetBrownColor(weight),
                MaterialColorKey.Grey => GetGreyColor(weight),
                MaterialColorKey.BlueGrey => GetBlueGreyColor(weight),
                _ => Color.white,
            };
        }

        private static Color GetRedColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(255, 235, 238, 255),
                MaterialColorWeight._100 => (Color)new Color32(255, 205, 210, 255),
                MaterialColorWeight._200 => (Color)new Color32(239, 154, 154, 255),
                MaterialColorWeight._300 => (Color)new Color32(229, 115, 115, 255),
                MaterialColorWeight._400 => (Color)new Color32(239, 83, 80, 255),
                MaterialColorWeight._500 => (Color)new Color32(244, 67, 54, 255),
                MaterialColorWeight._600 => (Color)new Color32(229, 57, 53, 255),
                MaterialColorWeight._700 => (Color)new Color32(211, 47, 47, 255),
                MaterialColorWeight._800 => (Color)new Color32(198, 40, 40, 255),
                MaterialColorWeight._900 => (Color)new Color32(183, 28, 28, 255),
                _ => (Color)new Color32(244, 67, 54, 255),
            };

        private static Color GetPinkColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(252, 228, 236, 255),
                MaterialColorWeight._100 => (Color)new Color32(248, 187, 208, 255),
                MaterialColorWeight._200 => (Color)new Color32(244, 143, 177, 255),
                MaterialColorWeight._300 => (Color)new Color32(240, 98, 146, 255),
                MaterialColorWeight._400 => (Color)new Color32(236, 64, 122, 255),
                MaterialColorWeight._500 => (Color)new Color32(233, 30, 99, 255),
                MaterialColorWeight._600 => (Color)new Color32(216, 27, 96, 255),
                MaterialColorWeight._700 => (Color)new Color32(194, 24, 91, 255),
                MaterialColorWeight._800 => (Color)new Color32(173, 20, 87, 255),
                MaterialColorWeight._900 => (Color)new Color32(136, 14, 79, 255),
                _ => (Color)new Color32(233, 30, 99, 255),
            };

        private static Color GetPurpleColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(243, 229, 245, 255),
                MaterialColorWeight._100 => (Color)new Color32(225, 190, 231, 255),
                MaterialColorWeight._200 => (Color)new Color32(206, 147, 216, 255),
                MaterialColorWeight._300 => (Color)new Color32(186, 104, 200, 255),
                MaterialColorWeight._400 => (Color)new Color32(171, 71, 188, 255),
                MaterialColorWeight._500 => (Color)new Color32(156, 39, 176, 255),
                MaterialColorWeight._600 => (Color)new Color32(142, 36, 170, 255),
                MaterialColorWeight._700 => (Color)new Color32(123, 31, 162, 255),
                MaterialColorWeight._800 => (Color)new Color32(106, 27, 154, 255),
                MaterialColorWeight._900 => (Color)new Color32(74, 20, 140, 255),
                _ => (Color)new Color32(156, 39, 176, 255),
            };

        private static Color GetDeepPurpleColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(237, 231, 246, 255),
                MaterialColorWeight._100 => (Color)new Color32(209, 196, 233, 255),
                MaterialColorWeight._200 => (Color)new Color32(179, 157, 219, 255),
                MaterialColorWeight._300 => (Color)new Color32(149, 117, 205, 255),
                MaterialColorWeight._400 => (Color)new Color32(126, 87, 194, 255),
                MaterialColorWeight._500 => (Color)new Color32(103, 58, 183, 255),
                MaterialColorWeight._600 => (Color)new Color32(94, 53, 177, 255),
                MaterialColorWeight._700 => (Color)new Color32(81, 45, 168, 255),
                MaterialColorWeight._800 => (Color)new Color32(69, 39, 160, 255),
                MaterialColorWeight._900 => (Color)new Color32(49, 27, 146, 255),
                _ => (Color)new Color32(103, 58, 183, 255),
            };

        private static Color GetIndigoColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(232, 234, 246, 255),
                MaterialColorWeight._100 => (Color)new Color32(197, 202, 233, 255),
                MaterialColorWeight._200 => (Color)new Color32(159, 168, 218, 255),
                MaterialColorWeight._300 => (Color)new Color32(121, 134, 203, 255),
                MaterialColorWeight._400 => (Color)new Color32(92, 107, 192, 255),
                MaterialColorWeight._500 => (Color)new Color32(63, 81, 181, 255),
                MaterialColorWeight._600 => (Color)new Color32(57, 73, 171, 255),
                MaterialColorWeight._700 => (Color)new Color32(48, 63, 159, 255),
                MaterialColorWeight._800 => (Color)new Color32(40, 53, 147, 255),
                MaterialColorWeight._900 => (Color)new Color32(26, 35, 126, 255),
                _ => (Color)new Color32(63, 81, 181, 255),
            };

        private static Color GetBlueColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(227, 242, 253, 255),
                MaterialColorWeight._100 => (Color)new Color32(187, 222, 251, 255),
                MaterialColorWeight._200 => (Color)new Color32(144, 202, 249, 255),
                MaterialColorWeight._300 => (Color)new Color32(100, 181, 246, 255),
                MaterialColorWeight._400 => (Color)new Color32(66, 165, 245, 255),
                MaterialColorWeight._500 => (Color)new Color32(33, 150, 243, 255),
                MaterialColorWeight._600 => (Color)new Color32(30, 136, 229, 255),
                MaterialColorWeight._700 => (Color)new Color32(25, 118, 210, 255),
                MaterialColorWeight._800 => (Color)new Color32(21, 101, 192, 255),
                MaterialColorWeight._900 => (Color)new Color32(13, 71, 161, 255),
                _ => (Color)new Color32(33, 150, 243, 255),
            };

        private static Color GetLightBlueColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(225, 245, 254, 255),
                MaterialColorWeight._100 => (Color)new Color32(179, 229, 252, 255),
                MaterialColorWeight._200 => (Color)new Color32(129, 212, 250, 255),
                MaterialColorWeight._300 => (Color)new Color32(79, 195, 247, 255),
                MaterialColorWeight._400 => (Color)new Color32(41, 182, 246, 255),
                MaterialColorWeight._500 => (Color)new Color32(3, 169, 244, 255),
                MaterialColorWeight._600 => (Color)new Color32(3, 155, 229, 255),
                MaterialColorWeight._700 => (Color)new Color32(2, 136, 209, 255),
                MaterialColorWeight._800 => (Color)new Color32(2, 119, 189, 255),
                MaterialColorWeight._900 => (Color)new Color32(1, 87, 155, 255),
                _ => (Color)new Color32(3, 169, 244, 255),
            };

        private static Color GetCyanColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(224, 247, 250, 255),
                MaterialColorWeight._100 => (Color)new Color32(178, 235, 242, 255),
                MaterialColorWeight._200 => (Color)new Color32(128, 222, 234, 255),
                MaterialColorWeight._300 => (Color)new Color32(77, 208, 225, 255),
                MaterialColorWeight._400 => (Color)new Color32(38, 198, 218, 255),
                MaterialColorWeight._500 => (Color)new Color32(0, 188, 212, 255),
                MaterialColorWeight._600 => (Color)new Color32(0, 172, 193, 255),
                MaterialColorWeight._700 => (Color)new Color32(0, 151, 167, 255),
                MaterialColorWeight._800 => (Color)new Color32(0, 131, 143, 255),
                MaterialColorWeight._900 => (Color)new Color32(0, 96, 100, 255),
                _ => (Color)new Color32(0, 188, 212, 255),
            };

        private static Color GetTealColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(224, 242, 241, 255),
                MaterialColorWeight._100 => (Color)new Color32(178, 223, 219, 255),
                MaterialColorWeight._200 => (Color)new Color32(128, 203, 196, 255),
                MaterialColorWeight._300 => (Color)new Color32(77, 182, 172, 255),
                MaterialColorWeight._400 => (Color)new Color32(38, 166, 154, 255),
                MaterialColorWeight._500 => (Color)new Color32(0, 150, 136, 255),
                MaterialColorWeight._600 => (Color)new Color32(0, 137, 123, 255),
                MaterialColorWeight._700 => (Color)new Color32(0, 121, 107, 255),
                MaterialColorWeight._800 => (Color)new Color32(0, 105, 92, 255),
                MaterialColorWeight._900 => (Color)new Color32(0, 77, 64, 255),
                _ => (Color)new Color32(0, 150, 136, 255),
            };

        private static Color GetGreenColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(232, 245, 233, 255),
                MaterialColorWeight._100 => (Color)new Color32(200, 230, 201, 255),
                MaterialColorWeight._200 => (Color)new Color32(165, 214, 167, 255),
                MaterialColorWeight._300 => (Color)new Color32(129, 199, 132, 255),
                MaterialColorWeight._400 => (Color)new Color32(102, 187, 106, 255),
                MaterialColorWeight._500 => (Color)new Color32(76, 175, 80, 255),
                MaterialColorWeight._600 => (Color)new Color32(67, 160, 71, 255),
                MaterialColorWeight._700 => (Color)new Color32(56, 142, 60, 255),
                MaterialColorWeight._800 => (Color)new Color32(46, 125, 50, 255),
                MaterialColorWeight._900 => (Color)new Color32(27, 94, 32, 255),
                _ => (Color)new Color32(76, 175, 80, 255),
            };

        private static Color GetLightGreenColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(241, 248, 233, 255),
                MaterialColorWeight._100 => (Color)new Color32(220, 237, 200, 255),
                MaterialColorWeight._200 => (Color)new Color32(197, 225, 165, 255),
                MaterialColorWeight._300 => (Color)new Color32(174, 213, 129, 255),
                MaterialColorWeight._400 => (Color)new Color32(156, 204, 101, 255),
                MaterialColorWeight._500 => (Color)new Color32(139, 195, 74, 255),
                MaterialColorWeight._600 => (Color)new Color32(124, 179, 66, 255),
                MaterialColorWeight._700 => (Color)new Color32(104, 159, 56, 255),
                MaterialColorWeight._800 => (Color)new Color32(85, 139, 47, 255),
                MaterialColorWeight._900 => (Color)new Color32(51, 105, 30, 255),
                _ => (Color)new Color32(139, 195, 74, 255),
            };

        private static Color GetLimeColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(249, 251, 231, 255),
                MaterialColorWeight._100 => (Color)new Color32(240, 244, 195, 255),
                MaterialColorWeight._200 => (Color)new Color32(230, 238, 156, 255),
                MaterialColorWeight._300 => (Color)new Color32(220, 231, 117, 255),
                MaterialColorWeight._400 => (Color)new Color32(212, 225, 87, 255),
                MaterialColorWeight._500 => (Color)new Color32(205, 220, 57, 255),
                MaterialColorWeight._600 => (Color)new Color32(192, 202, 51, 255),
                MaterialColorWeight._700 => (Color)new Color32(175, 180, 43, 255),
                MaterialColorWeight._800 => (Color)new Color32(158, 157, 36, 255),
                MaterialColorWeight._900 => (Color)new Color32(130, 119, 23, 255),
                _ => (Color)new Color32(205, 220, 57, 255),
            };

        private static Color GetYellowColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(255, 253, 231, 255),
                MaterialColorWeight._100 => (Color)new Color32(255, 249, 196, 255),
                MaterialColorWeight._200 => (Color)new Color32(255, 245, 157, 255),
                MaterialColorWeight._300 => (Color)new Color32(255, 241, 118, 255),
                MaterialColorWeight._400 => (Color)new Color32(255, 238, 88, 255),
                MaterialColorWeight._500 => (Color)new Color32(255, 235, 59, 255),
                MaterialColorWeight._600 => (Color)new Color32(253, 216, 53, 255),
                MaterialColorWeight._700 => (Color)new Color32(251, 192, 45, 255),
                MaterialColorWeight._800 => (Color)new Color32(249, 168, 37, 255),
                MaterialColorWeight._900 => (Color)new Color32(245, 127, 23, 255),
                _ => (Color)new Color32(255, 235, 59, 255),
            };

        private static Color GetAmberColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(255, 248, 225, 255),
                MaterialColorWeight._100 => (Color)new Color32(255, 236, 179, 255),
                MaterialColorWeight._200 => (Color)new Color32(255, 224, 130, 255),
                MaterialColorWeight._300 => (Color)new Color32(255, 213, 79, 255),
                MaterialColorWeight._400 => (Color)new Color32(255, 202, 40, 255),
                MaterialColorWeight._500 => (Color)new Color32(255, 193, 7, 255),
                MaterialColorWeight._600 => (Color)new Color32(255, 160, 0, 255),
                MaterialColorWeight._700 => (Color)new Color32(255, 143, 0, 255),
                MaterialColorWeight._800 => (Color)new Color32(255, 111, 0, 255),
                MaterialColorWeight._900 => (Color)new Color32(255, 61, 0, 255),
                _ => (Color)new Color32(255, 193, 7, 255),
            };

        private static Color GetOrangeColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(255, 243, 224, 255),
                MaterialColorWeight._100 => (Color)new Color32(255, 224, 178, 255),
                MaterialColorWeight._200 => (Color)new Color32(255, 204, 128, 255),
                MaterialColorWeight._300 => (Color)new Color32(255, 183, 77, 255),
                MaterialColorWeight._400 => (Color)new Color32(255, 167, 38, 255),
                MaterialColorWeight._500 => (Color)new Color32(255, 152, 0, 255),
                MaterialColorWeight._600 => (Color)new Color32(251, 140, 0, 255),
                MaterialColorWeight._700 => (Color)new Color32(245, 124, 0, 255),
                MaterialColorWeight._800 => (Color)new Color32(239, 108, 0, 255),
                MaterialColorWeight._900 => (Color)new Color32(230, 81, 0, 255),
                _ => (Color)new Color32(255, 152, 0, 255),
            };

        private static Color GetDeepOrangeColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(251, 233, 231, 255),
                MaterialColorWeight._100 => (Color)new Color32(255, 204, 188, 255),
                MaterialColorWeight._200 => (Color)new Color32(255, 171, 145, 255),
                MaterialColorWeight._300 => (Color)new Color32(255, 138, 101, 255),
                MaterialColorWeight._400 => (Color)new Color32(255, 112, 67, 255),
                MaterialColorWeight._500 => (Color)new Color32(255, 87, 34, 255),
                MaterialColorWeight._600 => (Color)new Color32(244, 81, 30, 255),
                MaterialColorWeight._700 => (Color)new Color32(230, 74, 25, 255),
                MaterialColorWeight._800 => (Color)new Color32(216, 67, 21, 255),
                MaterialColorWeight._900 => (Color)new Color32(191, 54, 12, 255),
                _ => (Color)new Color32(255, 87, 34, 255),
            };

        private static Color GetBrownColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(239, 235, 233, 255),
                MaterialColorWeight._100 => (Color)new Color32(215, 204, 200, 255),
                MaterialColorWeight._200 => (Color)new Color32(188, 170, 164, 255),
                MaterialColorWeight._300 => (Color)new Color32(161, 136, 127, 255),
                MaterialColorWeight._400 => (Color)new Color32(141, 110, 99, 255),
                MaterialColorWeight._500 => (Color)new Color32(121, 85, 72, 255),
                MaterialColorWeight._600 => (Color)new Color32(109, 76, 65, 255),
                MaterialColorWeight._700 => (Color)new Color32(93, 64, 55, 255),
                MaterialColorWeight._800 => (Color)new Color32(78, 52, 46, 255),
                MaterialColorWeight._900 => (Color)new Color32(62, 39, 35, 255),
                _ => (Color)new Color32(121, 85, 72, 255),
            };

        private static Color GetGreyColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(250, 250, 250, 255),
                MaterialColorWeight._100 => (Color)new Color32(245, 245, 245, 255),
                MaterialColorWeight._200 => (Color)new Color32(238, 238, 238, 255),
                MaterialColorWeight._300 => (Color)new Color32(224, 224, 224, 255),
                MaterialColorWeight._400 => (Color)new Color32(189, 189, 189, 255),
                MaterialColorWeight._500 => (Color)new Color32(158, 158, 158, 255),
                MaterialColorWeight._600 => (Color)new Color32(117, 117, 117, 255),
                MaterialColorWeight._700 => (Color)new Color32(97, 97, 97, 255),
                MaterialColorWeight._800 => (Color)new Color32(66, 66, 66, 255),
                MaterialColorWeight._900 => (Color)new Color32(33, 33, 33, 255),
                _ => (Color)new Color32(158, 158, 158, 255),
            };

        private static Color GetBlueGreyColor(MaterialColorWeight weight)
            => weight switch
            {
                MaterialColorWeight._50 => (Color)new Color32(236, 239, 241, 255),
                MaterialColorWeight._100 => (Color)new Color32(207, 216, 220, 255),
                MaterialColorWeight._200 => (Color)new Color32(176, 190, 197, 255),
                MaterialColorWeight._300 => (Color)new Color32(144, 164, 174, 255),
                MaterialColorWeight._400 => (Color)new Color32(120, 144, 156, 255),
                MaterialColorWeight._500 => (Color)new Color32(96, 125, 139, 255),
                MaterialColorWeight._600 => (Color)new Color32(84, 110, 122, 255),
                MaterialColorWeight._700 => (Color)new Color32(69, 90, 100, 255),
                MaterialColorWeight._800 => (Color)new Color32(55, 71, 79, 255),
                MaterialColorWeight._900 => (Color)new Color32(38, 50, 56, 255),
                _ => (Color)new Color32(96, 125, 139, 255),
            };
    }
}