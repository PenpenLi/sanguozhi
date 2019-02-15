﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CityBean {

    public string name;
    public Coordinates coordinate = new Coordinates();
    public int direction = 0; // 默认坐北朝南

    public City city;
}

public class CityData {
	
	static string CITY_MODEL = "Citys/City";
	static string PASS_MODEL = "Citys/Pass";
	static string PORT_MODEL = "Citys/Port";

    static string[] CITYS = {
        "武威","8","40",
        "安定","27","53",
        "陇西","9","69",
        "长安","52","72",
        "汉中","26","88",
        "梓潼","15","108",
        "成都","9","130",
        "江州","29","145",
        "建寧","26","173",
        "云南","4","190",
        "永安","45","126",

        "新野","86","108",
        "襄阳","76","116",
        "江陵","81","135",
        "江夏","114","120",
        "武陵","68","154",
        "零陵","69","183",
        "桂阳","98","187",
        "长沙","107","162",

        "柴桑","125","131",
        "庐江","145","116",
        "金陵","161","102",
        "吴","181","107",
        "会稽","183","123",

        "上庸","53","100",
        "宛","76","91",
        "洛阳","78","75",
        "陈留","107","77",
        "开封","119","69",
        "许昌","98","91",
        "汝南","112","106",

        "寿春","139","94",
        "小沛","135","75",
        "彭城","155","75",
        "临淄","159","49",

        "邺","108","52",
        "晋阳","90","38",
        "蓟","121","14",
        "南皮","137","35",
        "平原","128","53",
        "北平","148","19",
        "襄平","173","14",
    };

    static string[] PASSES = {
        "绵竹关","8","124","0",
        "涪水关","10","119","90",
        "葭萌关","23","101","0",
        "剑阁","14","96","0",
        "阳平关","27","82","0",
        "潼关","60","77","90",
        "函谷关","67","77","90",
        "武关","67","85","90",
        "虎牢关","89","76","90",
        "壶关","91","56","90",
    };

    static string[] PORTS = {
        "巫林港","54","129",
        "江津港","73","135",
        "公安港","74","145",
        "洞庭港","79","154",
        "罗阳港","100","147",
        "乌林港","99","132",
        "汉津港","89","124",
        "夏口港","108","122",
        "中庐港","70","109",
        "湖阳港","81","108",

        "房陵港","59","104",
        "夏阳港","59","54",
        "解阳港","65","64",
        "新丰港","64","73",
        "孟津港","79","69",
        "官渡港","101","71",
        "顿丘港","123","65",

        "陆口港","116","136",
        "九江港","132","128",
        "鄱阳港","146","141",
        "庐陵港","138","166",
        "皖口港","143","121",
        "芜湖港","163","108",
        "虎林港","161","121",
        "曲阿港","177","100",
        "句章港","187","117",

        "濡须港","148","109",
        "江都港","173","86",
        "海陵港","166","77",
        "冒阳港","176","47",
        "临济港","145","53",
        "高唐港","134","56",
        "白马港","110","62",
        "西河港","63","48",
        "安平港","168","27",
    };

    List<CityBean> mCitys = new List<CityBean>(); // 城市
    List<CityBean> mPasses = new List<CityBean>(); // 关口
    List<CityBean> mPorts = new List<CityBean>(); // 港口

    // 通过名字存放所有City
    Dictionary<string, City> mAllCitys = new Dictionary<string, City>();
    // 通过坐标存放所有City
    Dictionary<Coordinates, City> mAllCityCoordinates = new Dictionary<Coordinates, City>();

    public City GetCity(string name) {
        if (mAllCitys.ContainsKey(name)) {
            return mAllCitys[name];
        }
        return null;
    }

    public City GetCity(Coordinates coordinates) {
        if (mAllCityCoordinates.ContainsKey(coordinates)) {
            return mAllCityCoordinates[coordinates];
        }
        return null;
    }

    public void LoadData() {
		GameObject cityRoot = new GameObject("Citys");
		cityRoot.transform.position = new Vector3 (0,0,0);
        // 1.城市
        for (int i = 0; i < CITYS.Length;) {
            CityBean city = new CityBean();
            city.name = CITYS[i++];
            city.coordinate.x = int.Parse(CITYS[i++]) + 1;// 注意城池需要偏移
            city.coordinate.y = int.Parse(CITYS[i++]) + 2;
            mCitys.Add(city);
        }
        // 1-1 创建城市模型
        foreach (CityBean cityBean in mCitys) {
			GameObject o = GameObject.Instantiate(Resources.Load(CITY_MODEL)) as GameObject;
            o.transform.SetParent(cityRoot.transform);
            o.transform.localPosition = MapManager.GetInstance().CorrdinateToTerrainPosition(cityBean.coordinate);
            o.GetComponentInChildren<TextMesh>().text = cityBean.name;

            // cityComponent
            City cityComponent = o.AddComponent<City>();
            cityComponent.SetCityBean(cityBean);

            cityBean.city = cityComponent;
            mAllCitys.Add(cityBean.name, cityComponent);

            // 修改地形数据
            List<Coordinates> neighbours = MapManager.GetInstance().GetNeighbours(cityBean.coordinate);
            neighbours.Add(cityBean.coordinate);
            foreach (Coordinates coordinate in neighbours) {
                MapManager.GetInstance().GetMapDatas()[coordinate.x, coordinate.y] = (int)TerrainType.TerrainType_Dushi;
                mAllCityCoordinates.Add(coordinate , cityComponent);
            }
        }
        // 2.关口
        for (int i = 0; i < PASSES.Length;) {
            CityBean city = new CityBean();
            city.name = PASSES[i++];
            city.coordinate.x = int.Parse(PASSES[i++]);
            city.coordinate.y = int.Parse(PASSES[i++]);
            city.direction = int.Parse(PASSES[i++]);
            mPasses.Add(city);
        }
        // 2-1 创建关口模型
        foreach (CityBean cityBean in mPasses) {
			GameObject cityGameObject = GameObject.Instantiate(Resources.Load(PASS_MODEL)) as GameObject;
            cityGameObject.transform.SetParent(cityRoot.transform);
            cityGameObject.transform.localPosition = MapManager.GetInstance().CorrdinateToTerrainPosition(cityBean.coordinate);
            GameObject model = cityGameObject.transform.Find("Model").gameObject;
            //model.transform.localRotation = Quaternion.Euler(0, cityBean.direction, 0);
            model.transform.Rotate(new Vector3(0,0, cityBean.direction) , Space.Self);
            cityGameObject.GetComponentInChildren<TextMesh>().text = cityBean.name;
            // cityComponent
            City cityComponent = cityGameObject.AddComponent<City>();
            cityComponent.SetCityBean(cityBean);

            cityBean.city = cityComponent;
            mAllCitys.Add(cityBean.name, cityComponent);

            // 修改地形数据
            MapManager.GetInstance().GetMapDatas()[cityBean.coordinate.x, cityBean.coordinate.y] = (int)TerrainType.TerrainType_Guansuo;
            mAllCityCoordinates.Add(cityBean.coordinate, cityComponent);
        }
        // 3.港口
        for (int i = 0; i < PORTS.Length;) {
            CityBean city = new CityBean();
            city.name = PORTS[i++];
            city.coordinate.x = int.Parse(PORTS[i++]);
            city.coordinate.y = int.Parse(PORTS[i++]);
            //city.direction = int.Parse(PASSES[i++]);
            mPorts.Add(city);
        }
        // 3-1 创建港口模型
        foreach (CityBean cityBean in mPorts) {
			GameObject o = GameObject.Instantiate(Resources.Load(PORT_MODEL)) as GameObject;
            o.transform.SetParent(cityRoot.transform);
            o.transform.localPosition = MapManager.GetInstance().CorrdinateToTerrainPosition(cityBean.coordinate);
            o.GetComponentInChildren<TextMesh>().text = cityBean.name;

            // cityComponent
            City cityComponent = o.AddComponent<City>();
            cityComponent.SetCityBean(cityBean);

            cityBean.city = cityComponent;
            mAllCitys.Add(cityBean.name, cityComponent);

            // 修改地形数据
            MapManager.GetInstance().GetMapDatas()[cityBean.coordinate.x, cityBean.coordinate.y] = (int)TerrainType.TerrainType_Gang;
            mAllCityCoordinates.Add(cityBean.coordinate, cityComponent);
        }
    }

    public void AllocateWujiangData(WujiangData wujiangData){
        foreach (WujiangBean wujiangBean in wujiangData.GetAllWujiangs()) {
            if (mAllCitys.ContainsKey(wujiangBean.place)) {
                mAllCitys[wujiangBean.place].GetWujiangBeans().Add(wujiangBean);
            }
        }
    }
}