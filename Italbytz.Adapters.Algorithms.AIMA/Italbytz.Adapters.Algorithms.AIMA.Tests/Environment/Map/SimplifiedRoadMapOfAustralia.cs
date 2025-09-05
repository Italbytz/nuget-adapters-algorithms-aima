using Italbytz.AI.Tests.Environment.Map;

namespace aima.core.environment.map;

/**
 * Represents a simplified road map of Australia. The initialization method is
 * declared static. So it can also be used to initialize other specialized
 * subclasses of {@link ExtendableMap} with road map data from Australia. The
 * data was extracted from a class developed by Felix Knittel.
 *
 * @author Ruediger Lunde
 */
public class SimplifiedRoadMapOfAustralia : ExtendableMap
{
    // Locations
    public const string ADELAIDE = "Adelaide";
    public const string ALBANY = "Albany";
    public const string ALICE_SPRINGS = "AliceSprings";
    public const string BRISBANE = "Brisbane";
    public const string BROKEN_HILL = "BrokenHill";
    public const string BROOME = "Broome";
    public const string CAIRNS = "Cairns";
    public const string CAMARVON = "Camarvon";
    public const string CANBERRA = "Canberra";
    public const string CHARLEVILLE = "Charleville";
    public const string COOBER_PEDY = "CooberPedy";
    public const string DARWIN = "Darwin";
    public const string DUBBO = "Dubbo";
    public const string ESPERANCE = "Esperance";
    public const string GERALDTON = "Geraldton";
    public const string HALLS_CREEK = "HallsCreek";
    public const string HAY = "Hay";
    public const string KALGOORLIE = "Kalgoorlie";
    public const string KATHERINE = "Katherine";
    public const string LAKES_ENTRANCE = "LakesEntrance";
    public const string LONGREACH = "Longreach";
    public const string MACKAY = "Mackay";
    public const string MELBOURNE = "Melbourne";
    public const string MOUNT_GAMBIER = "MountGambier";
    public const string MT_ISA = "MtIsa";
    public const string NEWCASTLE = "Newcastle";
    public const string NORSEMAN = "Norseman";
    public const string NYNGAN = "Nyngan";
    public const string PERTH = "Perth";
    public const string PORT_AUGUSTA = "PortAugusta";
    public const string PORT_HEDLAND = "PortHedland";
    public const string PORT_LINCOLN = "PortLincoln";
    public const string PORT_MACQUARIE = "PortMacquarie";
    public const string ROCKHAMPTON = "Rockhampton";
    public const string SYDNEY = "Sydney";
    public const string TAMWORTH = "Tamworth";
    public const string TENNANT_CREEK = "TennantCreek";
    public const string TOWNSVILLE = "Townsville";
    public const string WAGGA_WAGGA = "WaggaWagga";
    public const string WARNAMBOOL = "Warnambool";
    public const string WYNDHAM = "Wyndham";

    public SimplifiedRoadMapOfAustralia()
    {
        initMap(this);
    }

    /**
         * Initializes a map with a simplified road map of Australia.
         */
    public static void initMap(ExtendableMap map)
    {
        map.Clear();
        // Add links
        // Distances from http://maps.google.com
        map.AddBidirectionalLink(PERTH, ALBANY, 417.0);
        map.AddBidirectionalLink(PERTH, KALGOORLIE, 593.0);
        map.AddBidirectionalLink(PERTH, GERALDTON, 424.0);
        map.AddBidirectionalLink(PERTH, PORT_HEDLAND, 1637.0);
        map.AddBidirectionalLink(ALBANY, ESPERANCE, 478.0);
        map.AddBidirectionalLink(KALGOORLIE, NORSEMAN, 187.0);
        map.AddBidirectionalLink(ESPERANCE, NORSEMAN, 204.0);
        map.AddBidirectionalLink(NORSEMAN, PORT_AUGUSTA, 1668.0);
        map.AddBidirectionalLink(GERALDTON, CAMARVON, 479.0);
        map.AddBidirectionalLink(CAMARVON, PORT_HEDLAND, 872.0);
        map.AddBidirectionalLink(PORT_HEDLAND, BROOME, 589.0);
        map.AddBidirectionalLink(BROOME, HALLS_CREEK, 685.0);
        map.AddBidirectionalLink(HALLS_CREEK, WYNDHAM, 370.0);
        map.AddBidirectionalLink(HALLS_CREEK, KATHERINE, 874.0);
        map.AddBidirectionalLink(WYNDHAM, KATHERINE, 613.0);
        map.AddBidirectionalLink(KATHERINE, DARWIN, 317.0);
        map.AddBidirectionalLink(KATHERINE, TENNANT_CREEK, 673.0);
        map.AddBidirectionalLink(TENNANT_CREEK, MT_ISA, 663.0);
        map.AddBidirectionalLink(TENNANT_CREEK, ALICE_SPRINGS, 508.0);
        map.AddBidirectionalLink(ALICE_SPRINGS, COOBER_PEDY, 688.0);
        map.AddBidirectionalLink(COOBER_PEDY, PORT_AUGUSTA, 539.0);
        map.AddBidirectionalLink(MT_ISA, TOWNSVILLE, 918.0);
        map.AddBidirectionalLink(TOWNSVILLE, CAIRNS, 346.0);
        map.AddBidirectionalLink(MT_ISA, LONGREACH, 647.0);
        map.AddBidirectionalLink(TOWNSVILLE, MACKAY, 388.0);
        map.AddBidirectionalLink(MACKAY, ROCKHAMPTON, 336.0);
        map.AddBidirectionalLink(LONGREACH, ROCKHAMPTON, 687.0);
        map.AddBidirectionalLink(ROCKHAMPTON, BRISBANE, 616.0);
        map.AddBidirectionalLink(LONGREACH, CHARLEVILLE, 515.0);
        map.AddBidirectionalLink(CHARLEVILLE, BRISBANE, 744.0);
        map.AddBidirectionalLink(CHARLEVILLE, NYNGAN, 657.0);
        map.AddBidirectionalLink(NYNGAN, BROKEN_HILL, 588.0);
        map.AddBidirectionalLink(BROKEN_HILL, PORT_AUGUSTA, 415.0);
        map.AddBidirectionalLink(NYNGAN, DUBBO, 166.0);
        map.AddBidirectionalLink(DUBBO, BRISBANE, 860.0);
        map.AddBidirectionalLink(DUBBO, SYDNEY, 466.0);
        map.AddBidirectionalLink(BRISBANE, TAMWORTH, 576.0);
        map.AddBidirectionalLink(BRISBANE, PORT_MACQUARIE, 555.0);
        map.AddBidirectionalLink(PORT_MACQUARIE, NEWCASTLE, 245.0);
        map.AddBidirectionalLink(TAMWORTH, NEWCASTLE, 284.0);
        map.AddBidirectionalLink(NEWCASTLE, SYDNEY, 159.0);
        map.AddBidirectionalLink(SYDNEY, CANBERRA, 287.0);
        map.AddBidirectionalLink(CANBERRA, WAGGA_WAGGA, 243.0);
        map.AddBidirectionalLink(DUBBO, WAGGA_WAGGA, 400.0);
        map.AddBidirectionalLink(SYDNEY, LAKES_ENTRANCE, 706.0);
        map.AddBidirectionalLink(LAKES_ENTRANCE, MELBOURNE, 317.0);
        map.AddBidirectionalLink(WAGGA_WAGGA, MELBOURNE, 476.0);
        map.AddBidirectionalLink(WAGGA_WAGGA, HAY, 269.0);
        map.AddBidirectionalLink(MELBOURNE, WARNAMBOOL, 269.0);
        map.AddBidirectionalLink(WARNAMBOOL, MOUNT_GAMBIER, 185.0);
        map.AddBidirectionalLink(MOUNT_GAMBIER, ADELAIDE, 449.0);
        map.AddBidirectionalLink(HAY, ADELAIDE, 655.0);
        map.AddBidirectionalLink(PORT_AUGUSTA, ADELAIDE, 306.0);
        map.AddBidirectionalLink(MELBOURNE, ADELAIDE, 728.0);
        map.AddBidirectionalLink(PORT_AUGUSTA, PORT_LINCOLN, 341.0);

        // Locations coordinates
        // Alice Springs is taken as central point with coordinates (0|0)
        // Therefore x and y coordinates refer to Alice Springs. Note that
        // the coordinates are not very precise and partly modified to
        // get a more real shape of Australia.
        map.SetPosition(ADELAIDE, 417, 1289);
        map.SetPosition(ALBANY, -1559, 1231);
        map.SetPosition(ALICE_SPRINGS, 0, 0);
        map.SetPosition(BRISBANE, 1882, 415);
        map.SetPosition(BROKEN_HILL, 709, 873);
        map.SetPosition(BROOME, -1189, -645);
        map.SetPosition(CAIRNS, 1211, -791);
        map.SetPosition(CAMARVON, -2004, -34);
        map.SetPosition(CANBERRA, 1524, 1189);
        map.SetPosition(CHARLEVILLE, 1256, 268);
        map.SetPosition(COOBER_PEDY, 86, 593);
        map.SetPosition(DARWIN, -328, -1237);
        map.SetPosition(DUBBO, 1474, 881);
        map.SetPosition(ESPERANCE, -1182, 1132);
        map.SetPosition(GERALDTON, -1958, 405);
        map.SetPosition(HALLS_CREEK, -630, -624);
        map.SetPosition(HAY, 985, 1143);
        map.SetPosition(KALGOORLIE, -1187, 729);
        map.SetPosition(KATHERINE, -183, -1025);
        map.SetPosition(LAKES_ENTRANCE, 1412, 1609);
        map.SetPosition(LONGREACH, 1057, -49);
        map.SetPosition(MACKAY, 1553, -316);
        map.SetPosition(MELBOURNE, 1118, 1570);
        map.SetPosition(MOUNT_GAMBIER, 602, 1531);
        map.SetPosition(MT_ISA, 563, -344);
        map.SetPosition(NEWCASTLE, 1841, 979);
        map.SetPosition(NORSEMAN, -1162, 881);
        map.SetPosition(NYNGAN, 1312, 781);
        map.SetPosition(PERTH, -1827, 814);
        map.SetPosition(PORT_AUGUSTA, 358, 996);
        map.SetPosition(PORT_HEDLAND, -1558, -438);
        map.SetPosition(PORT_LINCOLN, 169, 1205);
        map.SetPosition(PORT_MACQUARIE, 1884, 849);
        map.SetPosition(ROCKHAMPTON, 1693, -59);
        map.SetPosition(SYDNEY, 1778, 1079);
        map.SetPosition(TAMWORTH, 1752, 722);
        map.SetPosition(TENNANT_CREEK, 30, -445);
        map.SetPosition(TOWNSVILLE, 1318, -520);
        map.SetPosition(WAGGA_WAGGA, 1322, 1125);
        map.SetPosition(WARNAMBOOL, 761, 1665);
        map.SetPosition(WYNDHAM, -572, -932);
    }
}