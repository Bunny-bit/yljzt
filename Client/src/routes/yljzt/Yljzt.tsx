import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Form, Input, InputNumber, Badge, DatePicker, Row, Col,Icon } from 'antd';


const create = Form.create;
const FormItem = Form.Item;

const { TextArea } = Input;
import * as api from '../../api/api';

import CRUD from '../CRUD/CRUD';
import moment from 'moment';
import AddXuanxiang from './modal/AddXuanxiang'

function Yljzt({ form, record, visible, currentTimuId }) {
	const columns = [
		{
			title: '题号',
			dataIndex: 'tiHao',
			sorter: true
		},
		{
			title: '题目',
			dataIndex: 'tiMu',
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
			<FormItem label="题号" {...formCol}>
				{getFieldDecorator('tihao', {
					initialValue: record.tihao
				rules: [{ required: true, message: '请填写题号' }]
				})(<Input />)}
			</FormItem>
			<FormItem label="题目" {...formCol}>
				{getFieldDecorator('timu', {
					initialValue: record.timu,
					rules: [{ required: true, message: '请填写题目' }]
				})(<Input />)}
			</FormItem>

		</Form>
	);
	const filters = [
		{
			name: 'timu',
			displayName: '题目',
			option: 'like'
		},
	];

	const expandedRowRender = (r) => {

		return (<div>
			{
				r.xuanxiangs.map(x => (
					<Row>
						<Col span={1}>
							{x.isRight ? <Icon type="check-circle" /> : <Icon type="close-circle" />}
							{x.name}
						</Col>
						<Col  span={21}> {x.neirong}</Col>
						<Col  span={2}>



							<a onClick={() => {
								dispatch({
									type: 'yljzt/deleteXuanxiang',
									payload: {
										id: x.id
									}
								});

							}}>删除</a>
						</Col>
					</Row>
				))
			}
		</div>)
	}
	return (
		<div>
			<CRUD
				form={form}
				getAllApi={new api.YljztApi().appYljztGetAll}
				deleteApi={new api.YljztApi().appYljztDelete}
				updateApi={new api.YljztApi().appYljztUpdate}
				createApi={new api.YljztApi().appYljztCreate}
				columns={columns}
				formNode={formNode}

				customColumnOption={(t, r) => (
					<span>
						<a onClick={() => {
							dispatch({
								type: "yljzt/setState",
								payload: {
									visible: true,
									currentTimuId: r.id
								}
							})

						}} href="javascript:">
							增加选项
					</a>
					</span>
				)}
				filterProps={{
					filters,
					searchProvide: 'sql'
				}}

				tableProps={{ expandedRowRender: expandedRowRender }}
			/>
			{visible ? (<AddXuanxiang />) : null}
		</div>
	);
}

Yljzt = connect((state) => {
	return {
		...state.crud,
		visible: state.yljzt.visible,
		currentTimuId: state.yljzt.currentTimuId,
	};
})(Yljzt);
export default create()(Yljzt);
