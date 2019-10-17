import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Icon, Input, Button, Checkbox, notification, Row, Col } from 'antd';
import { ListView } from 'antd-mobile';
import { List, Radio, Flex, Picker } from 'antd-mobile';
import 'antd-mobile/dist/antd-mobile.css';
const FormItem = Form.Item;
const create = Form.create;
const RadioItem = Radio.RadioItem;
import styles from './Renyuas.css';

function Renyuas({ dispatch, form, isLoading, timus, answers, xueyuan }) {

    const { getFieldDecorator } = form;

    function handleSubmit(e) {

        form.validateFields((err, values) => {
            if (!err) {
                let data = {
                    "name": values.name,
                    "xueyua": values.xueyua.length ? values.xueyua[0] : "",
                    "xuehao": values.xuehao,
                    "banji": values.banji
                };
                let result = [];
                for (var i in timus) {
                    if (answers[timus[i].id]) {
                        result.push({
                            "timuId": timus[i].id,
                            "xuanxiangId": answers[timus[i].id]
                        })
                    }
                }
                data.answers = result;
                if (!data.answers || !data.answers.length || data.answers.length != 10) {
                    notification.error({
                        message: '提交失败',
                        description: '你还有没有作答的题目，请检查后再提交！'
                    });
                    return;
                }
                dispatch({
                    type: 'renyua/submit',
                    payload: data
                });
            }
        });

    }

    const formCol = {
        labelCol: { span: 8 },
        wrapperCol: { span: 12 }
    };


    let dataSource = new ListView.DataSource({
        rowHasChanged: (row1, row2) => row1 !== row2,
    });

    dataSource = dataSource.cloneWithRows(timus);

    const row = (rowData, sectionID, rowID) => {
        const obj = timus[rowID];
        return (
            <div key={rowID} style={{ padding: '0 15px' }}>
                <div style={{ lineHeight: 1.5 }}>
                    <List renderHeader={() => (<div className={styles.titles} style={{ fontSize: "20px" }}>{obj.title}</div>)}>
                        {obj.answers.map(v => {
                            return (
                                <RadioItem key={v.xuanxiangId}
                                    checked={answers[obj.id + ""] == v.xuanxiangId}
                                    onChange={() => {
                                        answers[obj.id + ""] = v.xuanxiangId;
                                        dispatch({
                                            type: "renyua/setState",
                                            payload: { answers: { ...answers } }
                                        });
                                        console.log(answers);
                                    }}>
                                    <span style={{ width: "80%", display: "inline-block", whiteSpace: "normal" }}>{v.name + " " + v.neirong}</span>
                                </RadioItem>
                            )
                        })}
                    </List>
                </div>
            </div>
        );
    };




    const separator = (sectionID, rowID) => (
        <div
            key={`${sectionID}-${rowID}`}
            style={{
                backgroundColor: '#7d1523',
                height: 8,
                borderTop: '1px solid #ECECED',
                borderBottom: '1px solid #ECECED',
            }}
        />
    );

    return (
        <div className={styles.container} >
            <img className={styles.images} src="http://www.ynctv.cn/ImageFile/GetPictureHvtThumbnailByPath?fileName=2/2019/6/18/logo.jpg" alt="" />
            <div className={styles.content} >
                <div className={styles.box}>
                    <section  >
                        <div className={styles.main}>

                            <ListView
                                dataSource={dataSource}
                                renderRow={row}
                                renderSeparator={separator}
                                className="am-list"
                                pageSize={4}
                                scrollRenderAheadDistance={500}
                                style={{ height: 180 * timus.length + "px" }}
                            />

                            <Form onSubmit={handleSubmit}>
                                <FormItem>
                                    <div className={styles.xinxi} >姓名：</div>
                                    {getFieldDecorator('name', {
                                        rules: [{ required: true, message: '请输入姓名名！' }]
                                    })(
                                        <Input className={styles.names}
                                        />
                                    )}
                                </FormItem>
                                <FormItem>
                                    <div className={styles.xinxi} >班级：</div>
                                    {getFieldDecorator('banji', {
                                        rules: [{ required: true, message: '请输入班级！' }]
                                    })(
                                        <Input className={styles.names}
                                        />
                                    )}
                                </FormItem>
                                <FormItem>
                                    <div className={styles.xinxi} >学号：</div>
                                    {getFieldDecorator('xuehao', {
                                        rules: [{ required: true, message: '请输入学号！' }]
                                    })(
                                        <Input className={styles.names}
                                        />
                                    )}
                                </FormItem>
                                <FormItem>
                                    <div className={styles.xinxi} >学院：</div>
                                    {getFieldDecorator('xueyua', {
                                        rules: [{ required: true, message: '请输入学院！' }]
                                    })(
                                        <Picker
                                            data={xueyuan}
                                            title="选择二级学院或教学部门"
                                        // value={this.state.sValue}
                                        // onChange={v => this.setState({ sValue: v })}
                                        // onOk={v => this.setState({ sValue: v })}
                                        >
                                            <List.Item arrow="horizontal">请选择</List.Item>
                                        </Picker>
                                    )}
                                </FormItem>

                                <Button type="primary" htmlType="submit" className={styles.contentl} >
                                    提交
					                </Button>
                            </Form>
                        </div>
                    </section>
                </div>
            </div>
        </div>
    );
}
Renyuas = connect((state) => {
    return {
        ...state.renyua,
    };
})(Renyuas);

export default create()(Renyuas);