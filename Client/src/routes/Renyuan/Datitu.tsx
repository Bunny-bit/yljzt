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
  chart.interval().position('xingming*timu').color('xingming');
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
        val = val * 100 + '%';
        return val;
      }
    }
  });

  chart.col('zhengquelv', {
    alias: '正确率(%)'
  });
  chart.col('xueyuan', {
    alias: '学院'
  });
  chart.intervalStack().position('zhengquelv').color('xueyuan').label('xueyuan',{
    formatter: function formatter(val, xueyuan) {
      return xueyuan.point.xueyuan + ': ' + val;
    }
  }).tooltip('xueyuan*zanbi', function(xueyuan, zanbi) {
    zanbi = zanbi * 100 + '%';
    return {
      name: xueyuan,
      value: zanbi
    };
  };
  chart.render();
});

const Chart2 = createG2(chart => {
  const Stat = G2.Stat;
  chart.setMode('select'); // 开启框选模式
  chart.select('rangeX'); // 设置 X 轴范围的框选
  chart.col('daduirengshu', {
    alias: '答对人数'
  });
  chart.col('timu', {
    alias: '题目'
  });
  chart.interval().position('timu*daduirengshu').color('timu');
  chart.render();
});

const Chart3 = createG2(chart => {
  const Stat = G2.Stat;
  chart.setMode('select'); // 开启框选模式
  chart.select('rangeX'); // 设置 X 轴范围的框选
  chart.col('canyurengshu', {
    alias: '参与人数'
  });
  chart.col('xueyuan', {
    alias: '学院'
  });
  chart.interval().position('xueyuan*canyurengshu').color('xueyuan');
  chart.render();
});

class Datitu extends React.Component {
  render() {
    const { data, value, shuju, cyrs, width, height, plotCfg, forceFit } = this.props;
    return (
      <div>
        <Row>
          <Col xl={11} lg={24} style={{ background: '#fff', marginTop: 20 }}>
            {<Chart
              data={data}
              width={width}
              height={height}
              plotCfg={plotCfg}
              forceFit={forceFit} />}</Col>
              <Col xl={{ span: 11, offset: 1 }} lg={24} style={{ background: '#fff', marginTop: 20 }}>
                {<Chart1
                  data={value}
                  width={width}
                  height={height}
                  plotCfg={plotCfg}
                  forceFit={forceFit} />}</Col>
              <Col xl={11} lg={24} style={{ background: '#fff', marginTop: 20 }}>
                {<Chart2
                  data={shuju}
                  width={width}
                  height={height}
                  plotCfg={plotCfg}
                  forceFit={forceFit} />}
              </Col>
              <Col xl={11} lg={24} style={{ background: '#fff', marginTop: 20 }}>
                {<Chart3
                  data={cyrs}
                  width={width}
                  height={height}
                  plotCfg={plotCfg}
                  forceFit={forceFit} />}
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