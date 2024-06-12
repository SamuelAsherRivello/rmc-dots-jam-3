namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Common values
    /// </summary>
    public static class ShootEmUp2DConstants
    {

        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------
        public static string GunShot01 = nameof(GunShot01);
        //
        public static string Pickup03 = nameof(Pickup03);
        //
        public const string GunHit03 = nameof(GunHit03);
 
        //
        public const float ScaleDownDuration = 0.25f;
        public const float ScaleUpDuration = 0.15f;
        
        //these use the same sound, so choose different sets of pitches
        public static float[] EnemyBulletGunHitPitches = { 0.7f, 0.8f, 0.9f, 1.0f };
        public static float[] PlayerBulletGunHitPitches = { 1.1f, 1.2f, 1.3f, 1.4f};
        
        //this is separate, choose any pitches
        public static float[] PickupHitPitches = { 1.1f, 1.2f, 1.3f};
        
        //this is separate, choose any pitches
        public static float[]  PlayerShootPitches =  { 0.8f, 0.9f, 1.0f, 1.1f };
    }
}