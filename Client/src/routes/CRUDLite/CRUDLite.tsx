import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import {
	Table,
	Button,
	Input,
	Modal,
	Checkbox,
	Tag,
	Row,
	Col,
	Badge,
	Popover,
	Icon,
	Popconfirm,
	Select,
	DatePicker,
	InputNumber,
	Form
} from 'antd';
const { Column, ColumnGroup } = Table;
const CheckboxGroup = Checkbox.Group;
const Option = Select.Option;
const Search = Input.Search;
const confirm = Modal.confirm;
import { remoteUrl } from '../../utils/url';
import Filter from '../../components/Filter/Filter';
import moment from 'moment';
import PermissionWrapper from './../../components/PermissionWrapper/PermissionWrapper';
const FormItem = Form.Item;

import Schema from 'async-validator';

class CRUDLite extends React.Component {
	static defaultProps = {
		useServiceUpdate: false,
		useColumnFilter: true,
		useColumnOption: true
	};
	state = {
		showColumns: [],
		columnSelectorVisible: false
	};
	componentDidMount() {
		this.props.dispatch({
			type: 'crudLite/getAll',
			payload: {
				api: this.props.getAllApi,
				data: {
					clear: true
				}
			}
		});
	}
	render() {
		const {
			dispatch,
			items,
			cacheItems,
			loading,
			saveLoading,
			pagination,
			form,
			formNode,
			columns,
			useColumnFilter,
			getAllApi,
			createApi,
			updateApi,
			deleteApi,
			toExcelApi,
			createPermission,
			updatePermission,
			deletePermission,
			toExcelPermission,
			useKeyWordFilter,
			filterProps = {},
			tableProps = {},
			deleteBatchApi,
			selectedIds,
			selectedRows,
			useColumnOption,
			perToolButtons,
			postToolButtons,
			onSaveing,
			customColumnOption,
			useRowSelection,
			onAfterSave
		} = this.props;
		let _this = this;
		function cancelColumnFilte() {
			_this.setState({
				showColumns: []
			});
		}
		const mapField = (text, record, column) => {
			switch (column.editable) {
				case 'boolean':
					return (
						<FormItem
							validateStatus={
								record.__validate[column.dataIndex] ? (
									record.__validate[column.dataIndex].validateStatus
								) : null
							}
							help={
								record.__validate[column.dataIndex] ? (
									record.__validate[column.dataIndex].errorMsg
								) : null
							}
						>
							<Checkbox
								defaultChecked={text}
								onChange={(e) => handleChange(e.target.checked, record.id, column.dataIndex)}
							/>
						</FormItem>
					);
				case 'datetime':
					return (
						<FormItem
							validateStatus={
								record.__validate[column.dataIndex] ? (
									record.__validate[column.dataIndex].validateStatus
								) : null
							}
							help={
								record.__validate[column.dataIndex] ? (
									record.__validate[column.dataIndex].errorMsg
								) : null
							}
						>
							{text ? (
								<DatePicker
									defaultValue={moment(text)}
									onChange={(e) => handleChange(e, record.id, column.dataIndex)}
								/>
							) : (
								<DatePicker onChange={(e) => handleChange(e, record.id, column.dataIndex)} />
							)}
						</FormItem>
					);
				case 'number':
					return (
						<FormItem
							validateStatus={
								record.__validate[column.dataIndex] ? (
									record.__validate[column.dataIndex].validateStatus
								) : null
							}
							help={
								record.__validate[column.dataIndex] ? (
									record.__validate[column.dataIndex].errorMsg
								) : null
							}
						>
							<InputNumber
								defaultValue={text}
								min={isNaN(column.min) ? -Infinity : column.min}
								max={isNaN(column.max) ? Infinity : column.max}
								onChange={(e) => handleChange(e, record.id, column.dataIndex)}
							/>
						</FormItem>
					);
				case 'select':
					return (
						<FormItem
							validateStatus={
								record.__validate[column.dataIndex] ? (
									record.__validate[column.dataIndex].validateStatus
								) : null
							}
							help={
								record.__validate[column.dataIndex] ? (
									record.__validate[column.dataIndex].errorMsg
								) : null
							}
						>
							<Select
								style={{ width: '100%' }}
								mode={column.multiple ? 'multiple' : ''}
								defaultValue={column.multiple ? text && text.length ? text : undefined : text}
								onChange={(e) => handleChange(e, record.id, column.dataIndex)}
							>
								{column.selectOptions.map((o, i) => (
									<Option key={i} value={o.value}>
										{o.name}
									</Option>
								))}
							</Select>
						</FormItem>
					);
				case 'custom':
					return column.customItem(record, handleChange);
				default:
					return (
						<FormItem
							validateStatus={
								record.__validate[column.dataIndex] ? (
									record.__validate[column.dataIndex].validateStatus
								) : null
							}
							help={
								record.__validate[column.dataIndex] ? (
									record.__validate[column.dataIndex].errorMsg
								) : null
							}
						>
							<Input
								defaultValue={text}
								onChange={(e) => handleChange(e.target.value, record.id, column.dataIndex)}
							/>
						</FormItem>
					);
			}
		};

		function renderColumns(text, record, column) {
			let showText = text;
			if (column.render) {
				showText = column.render(text, record);
			}
			return <div>{record.__editable && column.editable ? mapField(text, record, column) : showText}</div>;
		}

		let schemaObject = {};
		columns.map((n) => {
			if (n.editable && n.dataIndex && n.rules) {
				schemaObject[n.dataIndex] = n.rules;
			}
		});
		const schema = new Schema(schemaObject);

		function validate(target) {
			let result = true;
			schema.validate(target, (errors, fields) => {
				target.__validate = {};
				if (errors && errors.length) {
					errors.map((n) => {
						target.__validate[n.field] = {
							validateStatus: 'error',
							errorMsg: n.message
						};
					});
					result = false;
				}
			});
			return result;
		}
		function handleChange(value, id, name) {
			const newData = [ ...items ];
			const target = newData.filter((item) => id === item.id)[0];
			if (target) {
				if (name.indexOf('.') >= 0) {
					var strs = name.split('.');
					var obj = null;
					for (var i = 0; i < strs.length - 1; i++) {
						if (!target[strs[i]]) {
							target[strs[i]] = {};
						}
						obj = target[strs[i]];
					}
					obj[strs[strs.length - 1]] = value;
				} else {
					target[name] = value;
				}
				validate(target);
				dispatch({
					type: 'crudLite/setState',
					payload: {
						items: newData
					}
				});
			}
		}

		function add() {
			cancelColumnFilte();
			const newData = [ ...items ];
			if (newData.length > 0 && !newData[0].id) {
				return;
			}
			let addObject = {
				id: 0,
				__editable: true,
				__validate: {}
			};
			columns.map((n) => {
				if (n.dataIndex && n.editable) {
					addObject[n.dataIndex] = null;
				}
			});
			newData.unshift(addObject);
			dispatch({
				type: 'crudLite/setState',
				payload: {
					items: newData
				}
			});
		}

		function edit(id) {
			cancelColumnFilte();
			const newData = [ ...items ];
			const target = newData.filter((item) => id === item.id)[0];
			if (target) {
				target.__editable = true;
				target.__validate = {};
				this.setState({ data: newData });
				dispatch({
					type: 'crudLite/setState',
					payload: {
						items: newData,
						cacheItems: JSON.parse(JSON.stringify(newData))
					}
				});
			}
		}
		function save(id) {
			const newData = [ ...items ];
			const target = newData.filter((item) => id === item.id)[0];
			if (!validate(target)) {
				dispatch({
					type: 'crudLite/setState',
					payload: {
						items: newData
					}
				});
				return;
			}
			let saveObject = { ...target };
			if (onSaveing) {
				onSaveing(saveObject);
			}
			delete saveObject.__editable;
			delete saveObject.__validate;
			dispatch({
				type: target.id ? 'crudLite/update' : 'crudLite/create',
				payload: {
					data: saveObject,
					api: target.id ? updateApi : createApi,
					getAllApi: getAllApi,
					callback: (result) => {
						Object.assign(target, result);
						delete target.__editable;
						delete target.__validate;
						const newCacheData = [ ...cacheItems ];
						const cacheTarget = newCacheData.filter((item) => id === item.id)[0];
						if (cacheTarget) {
							Object.assign(cacheTarget, result);
						} else {
							newCacheData.unshift(result);
						}
						dispatch({
							type: 'crudLite/setState',
							payload: {
								items: newData,
								cacheItems: newCacheData
							}
						});
						if (onAfterSave) {
							onAfterSave(newData);
						}
					}
				}
			});
		}
		function cancelEdit(id) {
			const newData = [ ...items ];
			const target = newData.filter((item) => id === item.id)[0];
			if (target) {
				Object.assign(target, cacheItems.filter((item) => id === item.id)[0]);
				delete target.__editable;
				delete target.__validate;
				dispatch({
					type: 'crudLite/setState',
					payload: {
						items: newData
					}
				});
			}
		}
		function cancelAdd() {
			const newData = [ ...items ];
			if (!newData[0].id) {
				newData.shift();
				dispatch({
					type: 'crudLite/setState',
					payload: {
						items: newData
					}
				});
			}
		}

		function _onChange(pagination, filters, sorter) {
			dispatch({
				type: 'crudLite/getAll',
				payload: {
					data: {
						pagination: pagination,
						sorter: sorter
					},
					api: getAllApi
				}
			});
		}

		let _columns = columns.map((column) => ({
			...column,
			render: (text, record) => {
				return renderColumns(text, record, column);
			}
		}));
		if (useColumnOption && (updateApi || deleteApi || customColumnOption)) {
			_columns.push({
				title: '操作',
				render: (text, record) => (
					<span>
						{updateApi ? (
							<PermissionWrapper requiredPermission={updatePermission}>
								<span>
									{record.__editable ? (
										<span>
											<a onClick={save.bind(this, record.id)}>保存</a>
											<span style={{ marginLeft: '3px' }} />
											{record.id ? (
												<a onClick={cancelEdit.bind(this, record.id)}>取消编辑</a>
											) : (
												<a onClick={cancelAdd.bind(this)}>取消</a>
											)}
										</span>
									) : (
										<a onClick={edit.bind(this, record.id)}>编辑</a>
									)}
									<span style={{ marginLeft: '6px' }} />
								</span>
							</PermissionWrapper>
						) : null}
						{deleteApi && record.id ? (
							<PermissionWrapper requiredPermission={deletePermission}>
								<span>
									<Popconfirm
										title="你确定要删除吗?"
										onConfirm={() =>
											dispatch({
												type: 'crudLite/delete',
												payload: {
													data: {
														id: record.id
													},
													api: deleteApi,
													getAllApi: getAllApi
												}
											})}
									>
										<a href="#">删除</a>
									</Popconfirm>
									<span style={{ marginLeft: '6px' }} />
								</span>
							</PermissionWrapper>
						) : null}
						{customColumnOption ? customColumnOption(text, record) : null}
					</span>
				)
			});
		}
		let showColumns = [ ..._columns ];
		if (useColumnFilter && this.state.showColumns.length > 0) {
			showColumns = showColumns.filter((n) => this.state.showColumns.indexOf(n.title) >= 0);
		}
		const columnSelector = (
			<div>
				<div style={{ borderBottom: 1, width: 130 }}>
					<CheckboxGroup
						value={showColumns.map((n) => n.title)}
						onChange={(value) => {
							if (value.length >= 1) {
								this.setState({ showColumns: value });
							}
						}}
					>
						<Row>
							{_columns.map((n) => (
								<Col span={24} key={n.title}>
									<Checkbox value={n.title}>{n.title}</Checkbox>
								</Col>
							))}
						</Row>
					</CheckboxGroup>
				</div>
				<div>
					<center>
						<a
							onClick={() => {
								this.setState({ columnSelectorVisible: false });
							}}
						>
							关闭
						</a>
					</center>
				</div>
			</div>
		);

		const rowSelection = {
			onChange: (selectedRowKeys, selectedRows) => {
				dispatch({
					type: 'crudLite/setState',
					payload: {
						selectedIds: selectedRowKeys,
						selectedRows: selectedRows
					}
				});
			}
		};

		function deleteBatch() {
			if (!selectedIds || !selectedIds.length) {
				Modal.warning({ title: '未选中任何数据!' });
				return;
			}
			confirm({
				title: <div style={{ fontSize: '20px' }}>警告</div>,
				content: (
					<div>
						<div>你确定要删除吗?</div>
					</div>
				),
				width: 350,
				onOk() {
					dispatch({
						type: 'crudLite/deleteBatch',
						payload: {
							api: deleteBatchApi,
							data: { value: selectedIds },
							getAllApi: getAllApi
						}
					});
				},
				onCancel() {}
			});
		}

		function toExcel() {
			dispatch({
				type: 'crudLite/toExcel',
				payload: {
					api: toExcelApi
				}
			});
		}

		if (useKeyWordFilter) {
			filterProps.filters = [
				{
					name: 'filter',
					displayName: '输入关键字搜索'
				}
			];
		}

		const buttonStyle = {
			marginBottom: '12px',
			marginRight: '12px'
		};

		return (
			<div>
				<Filter
					onSearch={(value) =>
						dispatch({
							type: 'crudLite/getAll',
							payload: {
								api: this.props.getAllApi,
								data: {
									filters: value
								}
							}
						})}
					{...filterProps}
				/>
				<div>
					<div>
						{perToolButtons}
						{createApi ? (
							<PermissionWrapper requiredPermission={createPermission}>
								<Button type="primary" style={buttonStyle} onClick={add.bind(this)}>
									添加
								</Button>
							</PermissionWrapper>
						) : null}
						{deleteBatchApi ? (
							<PermissionWrapper requiredPermission={deletePermission}>
								<Button type="primary" style={buttonStyle} onClick={deleteBatch}>
									批量删除
								</Button>
							</PermissionWrapper>
						) : null}
						{toExcelApi ? (
							<PermissionWrapper requiredPermission={toExcelPermission}>
								<Button style={buttonStyle} onClick={toExcel}>
									导出到EXCEL
								</Button>
							</PermissionWrapper>
						) : null}
						{postToolButtons}

						{useColumnFilter ? (
							<Popover
								content={columnSelector}
								title={
									<div>
										<span>列表显示条目</span>
										<a
											onClick={() => {
												this.setState({ showColumns: [] });
											}}
											style={{ float: 'right' }}
										>
											恢复默认
										</a>
									</div>
								}
								placement="bottomRight"
								visible={this.state.columnSelectorVisible}
								trigger="click"
								onVisibleChange={() => {
									this.setState({ columnSelectorVisible: !this.state.columnSelectorVisible });
								}}
							>
								{items.filter((n) => n.__editable).length == 0 ? (
									<Button
										style={{ ...buttonStyle, float: 'right', marginRight: 0 }}
										onClick={() => {
											this.setState({ columnSelectorVisible: true });
										}}
									>
										自定义列表条目<Icon type="down" />
									</Button>
								) : (
									<Button
										disabled
										style={{ ...buttonStyle, float: 'right', marginRight: 0 }}
										onClick={() => {
											this.setState({ columnSelectorVisible: true });
										}}
									>
										自定义列表条目<Icon type="down" />
									</Button>
								)}
							</Popover>
						) : null}
					</div>
					<Table
						style={{ clear: 'both' }}
						dataSource={items}
						bordered={true}
						size="middle"
						rowKey={(record) => record.id}
						pagination={pagination}
						loading={loading}
						onChange={_onChange}
						columns={showColumns}
						rowSelection={useRowSelection || (deleteBatchApi && deletePermission) ? rowSelection : null}
						{...tableProps}
					/>
				</div>
			</div>
		);
	}
}
CRUDLite = connect((state) => {
	return {
		...state.crudLite,
		loading: state.loading.effects['crudLite/getAll'],
		saveLoading: state.loading.effects['crudLite/create'] || state.loading.effects['crudLite/update']
	};
})(CRUDLite);

export default CRUDLite;
