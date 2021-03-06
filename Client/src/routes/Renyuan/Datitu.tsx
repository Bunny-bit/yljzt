import G2 from 'g2';
import createG2 from 'g2-react';
import React, { Component } from 'react';
import { Row, Col } from 'antd';
import { connect } from 'dva';

const Chart = createG2(chart => {
  const Stat = G2.Stat;
  chart.setMode('select'); // 开启框选模式
  chart.select('rangeX'); // 设置 X 轴范围的框选
  chart.col('timu', {
    alias: '题目'
  });
  chart.col('xingming', {
    alias: '姓名'
  });
  chart.interval().position('xingming*timu').color('xingming').label('timu');
  chart.render();
});

const Chart1 = createG2(chart => {
  const Stat = G2.Stat;
  chart.setMode('select'); // 开启框选模式
  chart.select('rangeX'); // 设置 X 轴范围的框选

  chart.coord('theta', {
    radius: 0.75
  });
  chart.source(chart.data, {
    zhengquelv: {
      formatter: function formatter(val) {
        return val;
      }
    }
  });

  chart.col('renshu', {
    alias: '人数'
  });
  chart.col('xueyuan', {
    alias: '学院'
  });
  chart.intervalStack().position('zhanbi').color('xueyuan').label('xueyuan');
  chart.render();
});

const Chart2 = createG2(chart => {
  const Stat = G2.Stat;
  chart.setMode('select'); // 开启框选模式
  chart.select('rangeX'); // 设置 X 轴范围的框选
  chart.col('renshu', {
    alias: '答对人数'
  });
  chart.col('timu', {
    alias: '题目'
  });
  chart.interval().position('timu*renshu').color('timu').label('renshu');
  chart.render();
});

const Chart3 = createG2(chart => {
  const Stat = G2.Stat;
  chart.setMode('select'); // 开启框选模式
  chart.select('rangeX'); // 设置 X 轴范围的框选
  chart.col('zhengquelv', {
    alias: '正确率（%）'
  });
  chart.col('xuyuan', {
    alias: '学院'
  });
  chart.interval().position('xuyuan*zhengquelv').color('xuyuan').label('zhengquelv');
  chart.render();
});

class Datitu extends React.Component {
  render() {
    const { zhengQueShuZuiGao, canyuzhanbi, timuDaduiRenshu, zhengque, width, height, plotCfg, forceFit } = this.props;
    return (
      <div>
        <Row >
          <Col xl={12} lg={24} style={{ background: '#fff' }}>
            {
              zhengQueShuZuiGao && zhengQueShuZuiGao.length
                ? <Chart
                  data={zhengQueShuZuiGao}
                  width={width}
                  height={height}
                  plotCfg={plotCfg}
                  forceFit={forceFit} />
                : null
            }
          </Col>
          <Col xl={{ span: 12 }} lg={24} style={{ background: '#fff' }}>
            {
              canyuzhanbi && canyuzhanbi.length ? <Chart1
                data={canyuzhanbi}
                width={width}
                height={height}
                plotCfg={plotCfg}
                forceFit={forceFit} />
                : null
            }</Col>
          <Col xl={12} lg={24} style={{ background: '#fff' }}>
            {
              timuDaduiRenshu && timuDaduiRenshu.length ?
                <Chart2
                  data={timuDaduiRenshu}
                  width={width}
                  height={height}
                  plotCfg={plotCfg}
                  forceFit={forceFit} />
                : null
            }
          </Col>
          <Col xl={12} lg={24} style={{ background: '#fff' }}>
            {zhengque && zhengque.length ? <Chart3
              data={zhengque}
              width={width}
              height={height}
              plotCfg={plotCfg}
              forceFit={forceFit} />
              : null}
          </Col>
        </Row>
      </div>
    );
  }
}

Datitu = connect((state) => {//引入接口
  return {
    ...state.datitu
  };
})(Datitu);
export default Datitu;