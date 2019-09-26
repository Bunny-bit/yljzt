import G2 from 'g2';
import createG2 from 'g2-react';
import React, { Component } from 'react';
import { Row, Col } from 'antd';
import { connect } from 'dva';
import bingtu from '../../../models/bingtu';

// const Chart = createG2(chart => {
//   const Stat = G2.Stat;
//   chart.setMode('select'); // 开启框选模式
//   chart.select('rangeX'); // 设置 X 轴范围的框选
//   chart.col('timu', {
//     alias: '题目'
//   });
//   chart.col('zhengquelv', {
//     alias: '正确率(%)'
//   });
//   chart.interval().position('timu*zhengquelv').color('#e50000');
//   chart.render();
// });

//饼图
const Chart1 = createG2(chart1 => {
  const Stat = G2.Stat;
  chart1.setMode('select');
  chart1.select('rangeX');
  // 设置 X 轴范围的框选
  chart1.coord('theta',{
    radius:0.75
  });
  chart1.tooltip({
    showTitle: false
  });
  chart1.intervalStack().position('zhengquelv').color('#e50000').label('xueyuan', {
    formatter: function formatter(val, valuce) {
      return valuce.point.valuce + ': ' + val;
    }
  })
  // .tooltip('item*percent', function(item, percent) {
  //   percent = percent * 100 + '%';
  //   return {
  //     xueyuan: item,
  //     zhengquelv: percent
  //   }}
  ;
  chart1.render();
});


//柱状图
// class bingtu extends React.Component {
//   render() {
//     const { data, width, height, plotCfg, forceFit } = this.props;
//     return (
//       <div>
//         <Row>
//           <Col xl={8} lg={22} style={{ background: '#fff', marginTop: 20 }}>
//             <Chart
//               data={data}
//               width={width}
//               height={height}
//               plotCfg={plotCfg}
//               forceFit={forceFit} />
//           </Col>
//         </Row>
//       </div>

//     );

//   }

//饼图
class bingtu extends React.Component {
  render() {
    const { valuce, width, height } = this.props;
    return (
      <div>
        <Row>
          <Col xl={11} lg={24} style={{ background: '#fff', marginTop: 20 }}>
            <Chart1
              data={valuce}
              width={width}
              height={height} />
          </Col>
        </Row>
      </div>

    );
  }
}

bingtu = connect((state) => {//引入接口
  return {
    ...state.bingtu
  };
})(bingtu);
export default bingtu;