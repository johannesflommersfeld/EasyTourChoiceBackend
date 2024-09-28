namespace EasyTourChoice.API;

public enum Activity
{
    UNDEFINED,
    HIKING,
    TREKKING,
    BOULDERING,
    SPORTCLIMBING,
    MULTIPITCHCLIMBING,
    VIA_VERRATA,
    MOUNTAINBIKING,
    ROADCYCLING,
    GRAVEL,
    BIKEPACKING,
    SKITOURING,
}

public enum RiskLevel
{
    UNKNOWN,
    VERY_SAFE,
    SAFE,
    MODERATE_RISK,
    HIGH_RISK,
    DANGEROUS,
}

public enum AvelancheRisk
{
    UNKNOWN,
    LOW,
    MODERATE,
    CONSIDERABLE,
    HIGH,
    VERY_HIGH,
}

public enum WeatherPreview
{
    UNKNOWN,
    THUNDERSTORM,
    HEAVY_RAIN,
    RAIN,
    CLOUDY,
    PARTIALLY_SUNNY,
    PARTIALLY_SUNNY_WITH_RAIN,
    SUNNY,
}

public enum GeneralDifficulty
{
    UNKNOWN,
    EASY,
    MILDLY_CHALLENGING,
    CHALLENGING,
    VERY_CHALLENING
}

// uses a bitmap to allow for a combination of different aspects in a single field through an OR operation
public enum Aspect
{
    UNKNOWN = 0,
    [System.Runtime.Serialization.EnumMember(Value = @"N")]
    NORTH = 0b0000_0001,
    [System.Runtime.Serialization.EnumMember(Value = @"NE")]
    NORTH_EAST = 0b0000_0010,
    [System.Runtime.Serialization.EnumMember(Value = @"E")]
    EAST = 0b0000_0100,
    [System.Runtime.Serialization.EnumMember(Value = @"SE")]
    SOUTH_EAST = 0b0000_1000,
    [System.Runtime.Serialization.EnumMember(Value = @"S")]
    SOUTH = 0b0001_0000,
    [System.Runtime.Serialization.EnumMember(Value = @"SW")]
    SOUTH_WEST = 0b0010_0000,
    [System.Runtime.Serialization.EnumMember(Value = @"W")]
    WEST = 0b0100_0000,
    [System.Runtime.Serialization.EnumMember(Value = @"NW")]
    NORTH_WEST = 0b1000_0000,
}

// climbing & boldering scales
public enum VScale
{
    UNKNOWN,
    V1,
    V2,
    V3,
    V4,
    V5,
    V6,
    V7,
    V8,
    V10,
    V11,
    V12,
    V13,
    V14,
    V15,
    V16,
    V17,
}

public enum FontembleauScale
{
    unknown,
    FOUR,
    FIVE,
    FIVE_PLUS,
    SIX_A,
    SIX_A_PLUS,
    SIX_B,
    SIX_B_PLUS,
    SIX_C,
    SIX_C_PLUS,
    SEVEN_A,
    SEVEN_A_PLUS,
    SEVEN_B,
    SEVEN_B_PLUS,
    SEVEN_C,
    SEVEN_C_PLUS,
    EIGHT_A,
    EIGHT_A_PLUS,
    EIGHT_B,
    EIGHT_B_PLUS,
    EIGHT_C,
    EIGHT_C_PLUS,
    NINE_A,
}

public enum UIAAScale
{
    UNKNOWN,
    I,
    II,
    III,
    III_PLUS,
    IV_MINUS,
    IV,
    IV_PLUS,
    V_MINUS,
    V,
    VI_PLUS,
    VI_MINUS,
    VI,
    VII_PLUS,
    VII_MINUS,
    VII,
    VIII_PLUS,
    VIII_MINUS,
    VIII,
    V_PLUS,
    IX_MINUS,
    IX,
    IX_PLUS,
    X_MINUS,
    X,
    X_PLUS,
    XI_MINUS,
    XI,
    XI_PLUS,
    XII_MINUS,
    XII,
    XII_PLUS,
}

public enum FrenchScale
{
    UNKNOWN,
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE_A,
    FIVE_B,
    FIVE_C,
    SIX_A,
    SIX_A_PLUS,
    SIX_B,
    SIX_B_PLUS,
    SIX_C,
    SIX_C_PLUS,
    SEVEN_A,
    SEVEN_A_PLUS,
    SEVEN_B,
    SEVEN_B_PLUS,
    SEVEN_C,
    SEVEN_C_PLUS,
    EIGHT_A,
    EIGHT_A_PLUS,
    EIGHT_B,
    EIGHT_B_PLUS,
    EIGHT_C,
    EIGHT_C_PLUS,
    NINE_A,
    NINE_A_PLUS,
    NINE_B,
    NINE_B_PLUS,
    NINE_C,
}

public enum YosemityScale
{
    UNKNOWN,
    FIVE_TWO,
    FIVE_THREE,
    FIVE_FOUR,
    FIVE_FIVE,
    FIVE_SIX,
    FIVE_SEVEN,
    FIVE_EIGHT,
    FIVE_NINE,
    FIVE_TEN_A,
    FIVE_TEN_B,
    FIVE_TEN_C,
    FIVE_TEN_D,
    FIVE_ELEVEN_A,
    FIVE_ELEVEN_B,
    FIVE_ELEVEN_C,
    FIVE_ELEVEN_D,
    FIVE_TWELVE_A,
    FIVE_TWELVE_B,
    FIVE_TWELVE_C,
    FIVE_TWELVE_D,
    FIVE_THIRTEEN_A,
    FIVE_THIRTEEN_B,
    FIVE_THIRTEEN_C,
    FIVE_THIRTEEN_D,
    FIVE_FOURTEEN_A,
    FIVE_FOURTEEN_B,
    FIVE_FOURTEEN_C,
    FIVE_FOURTEEN_D,
    FIVE_FIFTEEN_A,
    FIVE_FIFTEEN_B,
    FIVE_FIFTEEN_C,
    FIVE_FIFTEEN_D,
}