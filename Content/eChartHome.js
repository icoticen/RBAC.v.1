var EChartOptionCircle = {
    tooltip: {
        trigger: 'item',
        formatter: "{a} <br/>{b}: {c} ({d}%)"
    },
    legend: {
        type: 'scroll',
        //orient: 'vertical',
        x: 'left',
        data: []
    },
    series: [
        {
            name: '访问来源',
            type: 'pie',
            selectedMode: 'single',
            radius: [0, '30%'],
            label: {
                normal: {
                    position: 'inner'
                }
            },
            data: [
                { value: 335, name: '直达' },
                { value: 679, name: '营销广告' },
                { value: 1548, name: '搜索引擎' }
            ]
        },
        {
            name: '访问来源',
            type: 'pie',
            radius: ['40%', '55%'],
            data: [
                { value: 335, name: '直达' },
                { value: 310, name: '邮件营销' },
                { value: 234, name: '联盟广告' },
                { value: 135, name: '视频广告' },
                { value: 1048, name: '百度' },
                { value: 251, name: '谷歌' },
                { value: 147, name: '必应' },
                { value: 102, name: '其他' }
            ]
        }
    ]
};
var EChartOptionLine = {
    title: {
        text: 'E-Chart @ViewBag.Title'
    },
    tooltip: {
        trigger: 'axis'
    },
    legend: {
        type: 'scroll',
        orient: 'vertical',
        top: '20%',
        right: '0',
        width: '150px',
        data: [],
    },
    dataZoom: [
        {
            show: true,
            realtime: true,
            start: 0,
            end: 100,
            xAxisIndex: [0],
            filterMode: 'filter',
        },
        {
            type: 'inside',
            realtime: true,
            start: 0,
            end: 100,
            xAxisIndex: [0],
            filterMode: 'filter',
        }
    ],
    grid: {
        left: '3%',
        right: '150px',
        bottom: '3%',
        top: '10%',
        height: '80%',
        containLabel: true
    },
    toolbox: {
        feature: {
            magicType: {
                type: ['line', 'bar', 'stack', 'tiled'],
            },
            dataZoom: {},
            dataView: { readOnly: true },
            saveAsImage: {},
            restore: {},
        },
    },
    xAxis: {
        type: 'category',
        boundaryGap: false,
        data: []
    },
    yAxis: {
        type: 'value'
    },
    series: [
    ],
}


var eChart_UserInAccountTypeOption = {
    tooltip: {
        trigger: 'item',
        formatter: "{a} <br/>{b}: {c} ({d}%)"
    },
    legend: {
        type: 'scroll',
        //orient: 'vertical',
        x: 'left',
        data: ['Mac', 'Phone', 'Mail', 'Account', 'WeXin', 'QQ', 'WeiBo', 'Other']
    },
    series: [
        {
            name: 'AccountType',
            type: 'pie',
            selectedMode: 'single',
            radius: [0, '80%'],
            label: {
                normal: {
                    position: 'inner'
                }
            },
            data: [
                { value: 5624, name: 'Account' },
                { value: 25478, name: 'Phone' },
                { value: 3941, name: 'Mail' },
                { value: 12245, name: 'WeXin' },
                { value: 6574, name: 'QQ' },
                { value: 9983, name: 'WeiBo' },
                { value: 8779, name: 'Other' },
                { value: 9577, name: 'Mac' },
            ]
        },    
    ]
};
var eChart_UserActionOption = {
    tooltip: {
        trigger: 'axis'
    },
    legend: {
        type: 'scroll',
        // orient: 'vertical',
        x: 'left',
        right: 130,
        data: ['LoginAct', 'LoginUser', 'RegisterAct'],
    },
    grid: {
        left: '5%',
        right: '5%',
        bottom: '3%',
        top: '15%',
        height: '80%',
        containLabel: true
    },
    toolbox: {
        feature: {
            magicType: {
                type: ['line', 'bar', 'stack', 'tiled'],
            },
            saveAsImage: {},
        },
    },
    xAxis: {
        type: 'category',
        boundaryGap: false,
        data: [(new Date().getMonth() + 1) + '-' + (new Date().getDate() - 6),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 5),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 4),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 3),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 2),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 1),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate()),
        ]
    },
    yAxis: {
        type: 'value'
    },
    series: [
         {
             name: 'LoginAct',
             type: 'line',
             data: [73564, 65479, 95542, 84751, 65514, 79951, 101024]
         },
         {
             name: 'LoginUser',
             type: 'line',
             data: [32248, 29457, 40027, 33547, 20197, 39994, 44210]
         },
        {
            name: 'RegisterAct',
            type: 'line',
            data: [270, 238, 264, 482, 340, 330, 380]
        },
    ],
};

var eChart_ApplicationInTypeOption = {
    tooltip: {
        trigger: 'item',
        formatter: "{a} <br/>{b}: {c} ({d}%)"
    },
    legend: {
        type: 'scroll',
        //orient: 'vertical',
        x: 'left',
        data: []
    },
    series: [
        {
            name: 'Type',
            type: 'pie',
            selectedMode: 'single',
            radius: [0, '50%'],
            label: {
                normal: {
                    position: 'inner'
                }
            },
            data: [
                { value: 85, name: 'RPG' },
                { value: 92, name: 'ACT' },
                { value: 42, name: 'AVG' },
                { value: 64, name: 'SLG' },
                { value: 126, name: 'RTS' },
                { value: 21, name: 'STG' },
                { value: 84, name: 'PZL' },
                { value: 113, name: 'OTH' },
            ]
        },
        {
            name: 'Type',
            type: 'pie',
            radius: ['65%', '85%'],
            data: [
                { value: 85, name: 'RPG' },

                { value: 92, name: 'ACT' },

                { value: 34, name: 'AVG' },
                { value: 8, name: 'AAG' },

                { value: 35, name: 'SLG' },
                { value: 7, name: 'SIM' },
                { value: 22, name: 'TCG' },

                { value:98, name: 'RTS' },
                { value:28, name: 'MOBA' },
                

                { value:13, name: 'STG' },
                { value:5, name: 'FPS' },
                { value:3, name: 'TPS' },

                { value:84, name: 'PZL' },

                { value:12, name: 'OTH' },
                { value:16, name: 'FTG' },
                { value:6, name: 'RCG' },
                { value:13, name: 'MSC' },
                { value:66, name: 'CAG' },


            ]
        }
    ]
};
var eChart_ApplicationActionOption = {
    tooltip: {
        trigger: 'axis'
    },
    legend: {
        type: 'scroll',
        // orient: 'vertical',
        x: 'left',
        right: 130,
        data: ['OpenAct', 'OpenUser', 'DownloadAct'],
    },
    grid: {
        left: '5%',
        right: '5%',
        bottom: '3%',
        top: '15%',
        height: '80%',
        containLabel: true
    },
    toolbox: {
        feature: {
            magicType: {
                type: ['line', 'bar', 'stack', 'tiled'],
            },
            saveAsImage: {},
        },
    },
    xAxis: {
        type: 'category',
        //boundaryGap: false,
        data: [(new Date().getMonth() + 1) + '-' + (new Date().getDate() - 6),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 5),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 4),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 3),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 2),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 1),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate()),
        ]
    },
    yAxis: {
        type: 'value'
    },
    series: [
         {
             name: 'OpenAct',
             type: 'bar',
             data: [93564, 85479, 115542, 104751, 85514, 99951, 121024]
         },
         {
             name: 'OpenUser',
             type: 'bar',
             data: [32248, 29457, 40027, 33547, 20197, 39994, 44210]
         },
        {
            name: 'DownloadAct',
            type: 'bar',
            data: [2705, 4238, 3264, 4482, 3340, 5330, 4380]
        },
    ],
};

var eChart_TradeActionOption = {
    tooltip: {
        trigger: 'axis'
    },
    legend: {
        type: 'scroll',
        // orient: 'vertical',
        x: 'left',
        right: 130,
        data: ['Amount', 'Count', 'User'],
    },
    grid: {
        left: '5%',
        right: '5%',
        bottom: '3%',
        top: '15%',
        height: '80%',
        containLabel: true
    },
    toolbox: {
        feature: {
            magicType: {
                type: ['line', 'bar', 'stack', 'tiled'],
            },
            saveAsImage: {},
        },
    },
    xAxis: {
        type: 'category',
        boundaryGap: false,
        data: [(new Date().getMonth() + 1) + '-' + (new Date().getDate() - 6),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 5),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 4),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 3),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 2),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate() - 1),
            (new Date().getMonth() + 1) + '-' + (new Date().getDate()),
        ]
    },
    yAxis: {
        type: 'value'
    },
    series: [
         {
             name: 'Amount',
             type: 'line',
             data: [7564, 6579, 9552, 8471, 6514, 7951, 11024]
         },
         {
             name: 'Count',
             type: 'line',
             data: [3248, 2957, 4027, 3347, 4197, 3994, 4210]
         },
        {
            name: 'User',
            type: 'line',
            data: [2370, 2368, 2644, 2282, 3440, 3630, 2880]
        },
    ],
};

