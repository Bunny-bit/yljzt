import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Input, InputNumber, Badge, DatePicker } from 'antd';

const create = Form.create;
const FormItem = Form.Item;

const { TextArea } = Input;
import * as api from '../../api/api';

import CRUD from '../CRUD/CRUD';
import moment from 'moment';

function Renyuan({ form, record }) {
	const columns = [
		{
			title: '姓名',
			dataIndex: 'xingming',
			sorter: true
		},
		{
			title: '班级',
			dataIndex: 'banji',
			sorter: true
		},
		{
			title: '学号',
			dataIndex: 'xuehao',
			sorter: true,
		},
		{
			title: '学院',
			dataIndex: 'xueyuan',
			sorter: true
		},
    ]
	const { getFieldDecorator } = form;
	const formCol = {
		labelCol: { span: 8 },
		wrapperCol: { span: 12 }
	};
	const formNode = (
		<Form>
			<FormItem label="姓名" {...formCol}>
				{getFieldDecorator('xingming', {
					initialValue: record.xingming,
					rules: [ { required: true, message: '请填写名称' } ]
				})(<Input />)}
			</FormItem>
			<FormItem label="班级" {...formCol}>
				{getFieldDecorator('banji', {
					initialValue: record.banji
				})(<TextArea />)}
			</FormItem>
			<FormItem label="学号" {...formCol}>
				{getFieldDecorator('xuehao', {
					initialValue: record.xuehao
				})(<InputNumber />)}
			</FormItem>
			<FormItem label="学院" {...formCol}>
				{getFieldDecorator('xueyuan', {
					initialValue: record.xueyuan
				})(<InputNumber />)}
			</FormItem>
		</Form>
	);
	const filters = [
		{
			name: 'xingming',
			displayName: '姓名',
			option: 'like'
		},
    ]
	return (
		<CRUD
			form={form}
			getAllApi={new api.RenyuanApi().appRenyuanGetAll}
			deleteApi={new api.RenyuanApi().appRenyuanDelete}
			updateApi={new api.RenyuanApi().appRenyuanUpdate}
			createApi={new api.RenyuanApi().appRenyuanCreate}
			columns={columns}
			formNode={formNode}
			filterProps={{
				filters,
				searchProvide: 'sql'
			}}
		/>
	);
}

Renyuan = connect((state) => {
	return {
		...state.crud
	};
})(Renyuan);
export default create()(Renyuan);
