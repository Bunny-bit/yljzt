import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Input, InputNumber, Badge, DatePicker } from 'antd';

const create = Form.create;
const FormItem = Form.Item;

const { TextArea } = Input;
import * as api from '../../api/api';

import CRUD from '../CRUD/CRUD';
import moment from 'moment';

function Xueyuan({ form, record }) {
	const columns = [
		{
			title: '名称',
			dataIndex: 'name',
			sorter: true
		}];
	const { getFieldDecorator } = form;
	const formCol = {
		labelCol: { span: 8 },
		wrapperCol: { span: 12 }
	};
	const formNode = (
		<Form>
			<FormItem label="名称" {...formCol}>
				{getFieldDecorator('name', {
					initialValue: record.name,
					rules: [{ required: true, message: '请填写名称' }]
				})(<Input />)}
			</FormItem>

		</Form>
	);
	const filters = [
		{
			name: 'name',
			displayName: '名称',
			option: 'like'
		},

	];

	return (
		<CRUD
			form={form}
			getAllApi={new api.XueyuanApi().appXueyuanGetAll}
			deleteApi={new api.XueyuanApi().appXueyuanDelete}
			updateApi={new api.XueyuanApi().appXueyuanUpdate}
			createApi={new api.XueyuanApi().appXueyuanCreate}
			columns={columns}
			formNode={formNode}
			filterProps={{
				filters,
				searchProvide: 'sql'
			}}
		/>
	);
}

Xueyuan = connect((state) => {
	return {
		...state.crud
	};
})(Xueyuan);
export default create()(Xueyuan);
