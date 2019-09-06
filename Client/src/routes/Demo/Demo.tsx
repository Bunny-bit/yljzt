import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Input, InputNumber, Badge, DatePicker } from 'antd';

const create = Form.create;
const FormItem = Form.Item;

const { TextArea } = Input;
import * as api from '../../api/api';

import CRUD from '../CRUD/CRUD';
import moment from 'moment';

function Demo({ form, record }) {
	const columns = [
		{
			title: '名称',
			dataIndex: 'name',
			sorter: true
		},
		{
			title: '长文本',
			dataIndex: 'longText',
			sorter: true
		},
		{
			title: '启用',
			dataIndex: 'isActivate',
			sorter: true,
			render: (text, record) =>
				text ? <Badge status="default" text="错误" /> : <Badge status="success" text="成功" />
		},
		{
			title: '排序',
			dataIndex: 'sort',
			sorter: true
		},
		{
			title: '经度',
			dataIndex: 'location.longitude',
			sorter: true
		},
		{
			title: '纬度',
			dataIndex: 'location.latitude',
			sorter: true
		},
		{
			title: '发布时间',
			dataIndex: 'publishTime',
			sorter: true,
			render: (text, record) => (text ? <span>{moment(text).format('YYYY-MM-DD HH:mm')}</span> : null)
		},
		{
			title: '添加时间',
			dataIndex: 'creationTime',
			sorter: true,
			render: (text, record) => (text ? <span>{moment(text).format('YYYY-MM-DD HH:mm')}</span> : null)
		},
		{
			title: '最后一次修改时间',
			dataIndex: 'lastModificationTime',
			sorter: true,
			render: (text, record) => (text ? <span>{moment(text).format('YYYY-MM-DD HH:mm')}</span> : null)
		}
	];
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
					rules: [ { required: true, message: '请填写名称' } ]
				})(<Input />)}
			</FormItem>
			<FormItem label="长文本" {...formCol}>
				{getFieldDecorator('longText', {
					initialValue: record.longText
				})(<TextArea />)}
			</FormItem>
			<FormItem label="排序" {...formCol}>
				{getFieldDecorator('sort', {
					initialValue: record.sort
				})(<InputNumber />)}
			</FormItem>
			<FormItem label="经度" {...formCol}>
				{getFieldDecorator('location.longitude', {
					initialValue: record.location && record.location.longitude
				})(<InputNumber />)}
			</FormItem>
			<FormItem label="纬度" {...formCol}>
				{getFieldDecorator('location.latitude', {
					initialValue: record.location && record.location.latitude
				})(<InputNumber />)}
			</FormItem>
			<FormItem label="发布时间" {...formCol}>
				{getFieldDecorator('publishTime', {
					initialValue: moment(record.publishTime)
				})(<DatePicker />)}
			</FormItem>
		</Form>
	);
	const filters = [
		{
			name: 'name',
			displayName: '名称',
			option: 'like'
		},
		{
			name: 'creationTime',
			displayName: '添加时间',
			type: 'datetime',
			option: '>='
		},
		{
			name: 'creationTime',
			displayName: '',
			type: 'datetime',
			option: '<'
		}
	];

	return (
		<CRUD
			form={form}
			getAllApi={new api.DemoApi().appDemoGetAll}
			deleteApi={new api.DemoApi().appDemoDelete}
			updateApi={new api.DemoApi().appDemoUpdate}
			createApi={new api.DemoApi().appDemoCreate}
			deleteBatchApi={new api.DemoApi().appDemoDeleteBatch}
			createPermission="Pages.DemoMangeCreate"
			updatePermission="Pages.DemoMangeUpdate"
			deletePermission="Pages.DemoMangeDelete"
			columns={columns}
			formNode={formNode}
			filterProps={{
				filters,
				searchProvide: 'sql'
			}}
		/>
	);
}

Demo = connect((state) => {
	return {
		...state.crud
	};
})(Demo);
export default create()(Demo);
