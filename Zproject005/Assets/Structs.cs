public struct VillagerData
{
    public string nombre;
    public int edad;

    static public implicit operator ZombieData(VillagerData villagerData)
    {
        ZombieData zombieData = new ZombieData();

        zombieData.nombre = villagerData.nombre;
        zombieData.edad = villagerData.edad;
        return zombieData;
    }

}

public struct ZombieData
{
    public string nombre;
    public string gusto;
    public int edad;
}
