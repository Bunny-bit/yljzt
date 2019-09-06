import G2 from 'g2';
import createG2 from 'g2-react';
import React, {Component} from 'react';
import {Row, Col} from 'antd';
import data from './echart.json';
import datas from './ditu.json';
function createData(source) {
  const now = new Date();
  const time = now.getTime();
  const temperature1 = ~~(Math.random() * 5) + 22;
  const temperature2 = ~~(Math.random() * 7) + 17;
  if (source.length >= 200) {
    source.shift();
    source.shift();
  }
  source.push({time: time, temperature: temperature1, type: '记录1'});
  source.push({time: time, temperature: temperature2, type: '记录2'});
  return source.concat();
}
const Chart = createG2(chart => {
  const Stat = G2.Stat;
  chart.setMode('select'); // 开启框选模式
  chart.select('rangeX'); // 设置 X 轴范围的框选
  chart.col('..count', {
    alias: 'top2000 唱片总量'
  });
  chart.col('release', {
    tickInterval: 5,
    alias: '唱片发行年份'
  });
  chart.interval().position(Stat.summary.count('release')).color('#e50000');
  chart.render();
  // 监听双击事件，这里用于复原图表
  chart.on('plotdblclick', function (ev) {
    chart.get('options').filters = {}; // 清空 filters
    chart.repaint();
  });
});
const Chart2 = createG2(chart => {
  chart.coord('polar', {inner: 0.1});
  chart.legend('难民类型', {
    position: 'bottom'
  });
  chart.intervalStack().position('year*count')
    .color('难民类型', ['rgb(136,186,174)', 'rgb(184,189,61)', 'rgb(107,136,138)'])
    .style({
      lineWidth: 1,
      stroke: '#fff'
    });
  chart.render();
});
const Chart3 = createG2(chart => {
  chart.col('time', {
    alias: '时间',
    type: 'time',
    mask: 'MM:ss',
    tickCount: 10,
    nice: false
  });
  chart.col('temperature', {
    alias: '平均温度(°C)',
    min: 10,
    max: 35
  });
  chart.col('type', {
    type: 'cat'
  });
  chart.line().position('time*temperature').color('type', ['#ff7f0e', '#2ca02c']).shape('smooth').size(2);
  chart.render();
});
let chinaGeoJSON;
const Chart4 = createG2(chart => {
  const Stat = G2.Stat;
  chart.axis(false);
  chart.polygon().position(Stat.map.region('name', chinaGeoJSON))
    .color('value', '#F4EC91-#AF303C')
    .label('name', {label: {fill: '#000', shadowBlur: 5, shadowColor: '#fff'}})
    .style({
      stroke: '#333',
      lineWidth: 1
    });
  chart.render();
});
const Echartall = React.createClass({
  getInitialState() {
    let source = [];
    var data2 = [
      {year: '2000', internally: 21.0, refugees: 16, seekers: 0.8},
      {year: '2001', internally: 25.0, refugees: 16, seekers: 0.8},
      {year: '2002', internally: 25.0, refugees: 15, seekers: 0.8},
      {year: '2003', internally: 25.0, refugees: 14, seekers: 0.7},
      {year: '2004', internally: 25.0, refugees: 14, seekers: 0.7},
      {year: '2005', internally: 24.0, refugees: 13, seekers: 0.8},
      {year: '2006', internally: 24.0, refugees: 14, seekers: 0.7},
      {year: '2007', internally: 26.0, refugees: 16, seekers: 0.7},
      {year: '2008', internally: 26.0, refugees: 15.2, seekers: 0.8},
      {year: '2009', internally: 27.1, refugees: 15.2, seekers: 1.0},
      {year: '2010', internally: 27.5, refugees: 15.4, seekers: 0.8},
      {year: '2011', internally: 26.4, refugees: 15.2, seekers: 0.9},
      {year: '2012', internally: 28.8, refugees: 15.4, seekers: 0.9},
      {year: '2013', internally: 33.3, refugees: 16.7, seekers: 1.2},
      {year: '2014', internally: 38.2, refugees: 19.5, seekers: 1.8}
    ];
    var Frame = G2.Frame;
    var frame = new Frame(data2); // 加工数据
    frame = Frame.combinColumns(frame, ['internally', 'refugees', 'seekers'], 'count', '难民类型', 'year');
    return {
      data: [],
      data2: frame,
      data3: createData(source),
      forceFit: true,
      width: 500,
      height: 450,
      plotCfg: {
        margin: [20, 60, 80, 120]
      },
    };
  },
  componentWillUnmount: function () {
    //离开销毁计时器
    this.timer1 && clearTimeout(this.timer1);
  },
  componentDidMount: function () {
    const self = this;
    const Frame2 = G2.Frame;
    let frame2 = new Frame2(data);
    frame2 = Frame2.sort(frame2, 'release');
    self.setState({
      data: frame2
    });
    this.timer1 = setInterval(function () {
      var data3 = self.state.data3;
      // console.log(data3);
      self.setState({
        data3: createData(data3)
      });
    }, 1000);
    // axios.get('https://antv.alipay.com/static/data/china.json').then(function (response) {
    chinaGeoJSON = datas;
    const userData = [];
    const features = chinaGeoJSON.features;
    for (let i = 0; i < features.length; i++) {
      const name = features[i].properties.name;
      userData.push({
        "name": name,
        "value": Math.round(Math.random() * 1000)
      });
    }
    self.setState({
      data4: userData
    });
    // }).catch(function (error) {
    //   console.log(error);
    // });

  },
  render() {
    return (
      <div>
        <Row>
          <Col xl={11} lg={24} style={{background: '#fff', marginTop: 20}}><Chart
            data={this.state.data}
            width={this.state.width}
            height={this.state.height}
            plotCfg={this.state.plotCfg}
            forceFit={this.state.forceFit}/></Col>
          <Col xl={{span: 11, offset: 1}} lg={24} style={{background: '#fff', marginTop: 20}}><Chart2
            data={this.state.data2}
            width={this.state.width}
            height={this.state.height}
            plotCfg={this.state.plotCfg}
            forceFit={this.state.forceFit}/></Col>
          <Col xl={11} lg={24} style={{background: '#fff', marginTop: 20}}> <Chart3
            data={this.state.data3}
            width={this.state.width}
            height={this.state.height}
            forceFit={this.state.forceFit}/></Col>

          {this.state.data.length === 0 ? <div></div> :
            <Col xl={{span: 11, offset: 1}} lg={24} style={{background: '#fff', marginTop: 20}}> <Chart4
              data={this.state.data4}
              width={this.state.width}
              height={this.state.height}
              forceFit={this.state.forceFit}/></Col>}
        </Row>
      </div>
    );
  },
});

export default Echartall;
