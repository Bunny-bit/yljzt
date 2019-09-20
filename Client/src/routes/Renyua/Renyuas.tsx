import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Icon, Input, Button, Checkbox, message, Row, Col } from 'antd';
import { ListView } from 'antd-mobile';
import { List, Radio, Flex, WhiteSpace } from 'antd-mobile';
import 'antd-mobile/dist/antd-mobile.css';
const FormItem = Form.Item;
const create = Form.create;
const RadioItem = Radio.RadioItem;
import styles from './Renyuas.css';

function Renyuas({ dispatch, form, isLoading, data, values }) {

    const { getFieldDecorator } = form;

    function handleSubmit(e) {

        form.validateFields((err, values) => {
            let data = {
                "name": values.name,
                "xueyua": values.xueyua,
                "xuehao": values.xuehao,
                "banji": values.banji
            };
            if (!err) {
                dispatch({
                    type: 'renyua/createrenyua',
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


    dataSource = dataSource.cloneWithRows(data);

    const row = (rowData, sectionID, rowID) => {
        const obj = data[rowID];
        return (
            <div key={rowID} style={{ padding: '0 0' }}>
                <div>
                    <div style={{ lineHeight: 1 }}>
                        <List renderHeader={() => { return obj.title }}>
                            {obj.answers.map(v => {
                                return (
                                    <RadioItem key={v}
                                        checked={values[obj.id + ""] === v}
                                        onChange={() => {
                                            values[obj.id + ""] = v;
                                            dispatch({
                                                type: "renyua/setState",
                                                payload: { values: { ...values } }
                                            });
                                        }}>
                                        {v}
                                    </RadioItem>
                                )
                            })
                            }
                        </List>


                    </div>
                </div>
            </div>
        );
    };




    const separator = (sectionID, rowID) => (
        <div
            key={`${sectionID}-${rowID}`}
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
                                style={{ height: 247 * data.length + "px" }}
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
                                        <Input className={styles.names}
                                        />
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