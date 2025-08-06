public static class MathHelper
{
    public static int MathematicalMod(int x, int m)
    {
        int r = x % m;

        return r < 0 ? r + m : r;
    }
}
