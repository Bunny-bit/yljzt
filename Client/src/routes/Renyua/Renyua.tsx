import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Input, InputNumber, Badge, DatePicker } from 'antd';

const create = Form.create;
const FormItem = Form.Item;

const { TextArea } = Input;
import * as api from '../../api/api';

import CRUD from '../CRUD/CRUD';
import moment from 'moment';

function Renyua({ form, record }) {
    const columns = [
        {
            title: '姓名',
            dataIndex: 'name',
            sorter: true
        },
        {
            title: '学院',
            dataIndex: 'xueyua',
            sorter: true
        },
        {
            title: '学号',
            dataIndex: 'xuehao',
            sorter: true,
        },
        {
            title: '班级',
            dataIndex: 'bankji',
            sorter: true
        },
    ];
    const { getFieldDecorator } = form;
    const formCol = {
        labelCol: { span: 8 },
        wrapperCol: { span: 12 }
    };
    const formNode = (
        <Form>
            <FormItem label="姓名" {...formCol}>
                {getFieldDecorator('name', {
                    initialValue: record.name,
                    rules: [{ required: true, message: '请填写名称' }]
                })(<Input />)}
            </FormItem>
            <FormItem label="学院" {...formCol}>
                {getFieldDecorator('xueyua', {
                    initialValue: record.longText
                })(<input/>)}
            </FormItem>
            <FormItem label="学号" {...formCol}>
                {getFieldDecorator('xuehao', {
                    initialValue: record.sort
                })(<InputNumber />)}
            </FormItem>
            <FormItem label="班级" {...formCol}>
                {getFieldDecorator('bankji', {
                    initialValue: record.location && record.location.longitude
                })(<Input />)}
            </FormItem>
        </Form>
    );
    const filters = [
        {
            name: 'name',
            displayName: '姓名',
            option: 'like'
        },
    ];

    return (
        <CRUD
            form={form}
            getAllApi={new api.RenyuaApi().appRenyuaGetAll}
            deleteApi={new api.RenyuaApi().appRenyuaDelete}
            updateApi={new api.RenyuaApi().appRenyuaUpdate}
            createApi={new api.RenyuaApi().appRenyuaCreate}
            columns={columns}
            formNode={formNode}
            filterProps={{
                filters,
                searchProvide: 'sql'
            }}
        />
    );
}

Renyua = connect((state) => {
    return {
        ...state.crud
    };
})(Renyua);
export default create()(Renyua);
