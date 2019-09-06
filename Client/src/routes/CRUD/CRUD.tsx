import React, { Component, PropTypes } from 'react';
import { connect } from 'dva';
import { Table, Button, Input, Modal, Checkbox, Tag, Row, Col, Badge, Popover, Icon, Menu, Dropdown } from 'antd';
const { Column, ColumnGroup } = Table;
const CheckboxGroup = Checkbox.Group;
const Search = Input.Search;
const confirm = Modal.confirm;
import { remoteUrl } from '../../utils/url';
import Filter from '../../components/Filter/Filter';
import PermissionWrapper from './../../components/PermissionWrapper/PermissionWrapper';

class CRUD extends React.Component {
	static defaultProps = {
		modalWidth: 520,
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
			type: 'crud/getAll',
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
			loading,
			saveLoading,
			pagination,
			modalVisible,
			modalWidth,
			form,
			modalText,
			isAdd,
			record,
			formNode,
			columns,
			useColumnFilter,
			getAllApi,
			createApi,
			updateApi,
			deleteApi,
			getApi,
			toExcelApi,
			createPermission,
			updatePermission,
			deletePermission,
			toExcelPermission,
			useServiceUpdate,
			useKeyWordFilter,
			filterProps = {},
			tableProps = {},
			modalProps = {},
			deleteBatchApi,
			selectedIds,
			selectedRows,
			useColumnOption,
			customColumnOption,
			perToolButtons,
			postToolButtons,
			onModalOpen,
			isCollapseToolButton,
			isEditable,
			onSaveing,
			showRowSelection = false
		} = this.props;
		function showAddModal() {
			if (onModalOpen) {
				onModalOpen(null);
			}
			dispatch({
				type: 'crud/setState',
				payload: {
					modalVisible: true,
					modalText: '添加',
					isAdd: true,
					record: {}
				}
			});
		}

		function showEditModal(record) {
			if (onModalOpen) {
				onModalOpen(record);
			}
			form.resetFields();
			if (useServiceUpdate) {
				dispatch({
					type: 'crud/get',
					payload: {
						data: { id: record.id },
						api: getApi,
						form: form
					}
				});
			} else {
				dispatch({
					type: 'crud/setState',
					payload: {
						modalVisible: true,
						record: record,
						modalText: '编辑',
						isAdd: false
					}
				});

				for (var key in record) {
					form.getFieldDecorator(key, { initialValue: record[key] });
				}
			}
		}

		function showConfirm(id, record) {
			confirm({
				title: '警告',
				content: (
					<div>
						<div>你确定要删除吗?</div>
					</div>
				),
				width: 350,
				onOk() {
					dispatch({
						type: 'crud/delete',
						payload: {
							data: {
								id: id
							},
							api: deleteApi,
							getAllApi: getAllApi
						}
					});
				},
				onCancel() {}
			});
		}
		function save() {
			form.validateFields((err, values) => {
				if (!err) {
					if (onSaveing) {
						onSaveing(values);
					}
					if (!isAdd) {
						values.id = record.id;
						dispatch({
							type: 'crud/update',
							payload: {
								data: values,
								api: updateApi,
								getAllApi: getAllApi
							}
						});
					} else {
						dispatch({
							type: 'crud/create',
							payload: {
								data: values,
								api: createApi,
								getAllApi: getAllApi
							}
						});
					}
				}
			});
		}

		function _onChange(pagination, filters, sorter) {
			console.log(sorter);
			dispatch({
				type: 'crud/getAll',
				payload: {
					data: {
						pagination: pagination,
						sorter: sorter
					},
					api: getAllApi
				}
			});
		}

		function handleCancel() {
			dispatch({
				type: 'crud/setState',
				payload: {
					modalVisible: false
				}
			});
		}

		const formCol = {
			labelCol: { span: 8 },
			wrapperCol: { span: 12 }
		};
		let _columns = [ ...columns ];
		if (useColumnOption && (updateApi || deleteApi || customColumnOption)) {
			_columns.push({
				title: '操作',
				render: (text, record) => {
					if (isCollapseToolButton) {
						const menu = (
							<PermissionWrapper>
								<Menu>
									{updateApi && (!isEditable || isEditable(record)) ? (
										<Menu.Item requiredPermission={updatePermission}>
											<a onClick={showEditModal.bind(this, record)}>编辑</a>
										</Menu.Item>
									) : null}
									{deleteApi ? (
										<Menu.Item requiredPermission={deletePermission}>
											<a onClick={showConfirm.bind(this, record.id, record)}>删除</a>
										</Menu.Item>
									) : null}
									{customColumnOption ? customColumnOption(text, record) : null}
								</Menu>
							</PermissionWrapper>
						);
						return (
							<PermissionWrapper>
								<Dropdown overlay={menu}>
									<a className="ant-dropdown-link">
										操作 <Icon type="down" />
									</a>
								</Dropdown>
							</PermissionWrapper>
						);
					} else {
						return (
							<span>
								{updateApi ? (
									<PermissionWrapper requiredPermission={updatePermission}>
										<span>
											<a onClick={showEditModal.bind(this, record)}>编辑</a>
											<span style={{ marginLeft: '6px' }} />
										</span>
									</PermissionWrapper>
								) : null}
								{deleteApi ? (
									<PermissionWrapper requiredPermission={deletePermission}>
										<span>
											<a onClick={showConfirm.bind(this, record.id, record)}>删除</a>
											<span style={{ marginLeft: '6px' }} />
										</span>
									</PermissionWrapper>
								) : null}
								{customColumnOption ? customColumnOption(text, record) : null}
							</span>
						);
					}
				}
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
					type: 'crud/setState',
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
				title: '警告',
				content: (
					<div>
						<div>你确定要删除吗?</div>
					</div>
				),
				width: 350,
				onOk() {
					dispatch({
						type: 'crud/deleteBatch',
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
				type: 'crud/toExcel',
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
							type: 'crud/getAll',
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
								<Button type="primary" style={buttonStyle} onClick={showAddModal.bind(this)}>
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
								<Button
									style={{ ...buttonStyle, float: 'right', marginRight: 0 }}
									onClick={() => {
										this.setState({ columnSelectorVisible: true });
									}}
								>
									自定义列表条目<Icon type="down" />
								</Button>
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
						rowSelection={(deleteBatchApi && deletePermission) || showRowSelection ? rowSelection : null}
						{...tableProps}
					/>
				</div>
				<Modal
					width={modalWidth}
					visible={modalVisible}
					title={modalText}
					onCancel={handleCancel}
					footer={[
						<Button key="save" loading={saveLoading} type="primary" onClick={save}>
							保存
						</Button>,
						<Button key="cancel" onClick={handleCancel}>
							取消
						</Button>
					]}
					{...modalProps}
				>
					{modalVisible ? formNode : null}
				</Modal>
			</div>
		);
	}
}
CRUD = connect((state) => {
	return {
		...state.crud,
		loading: state.loading.effects['crud/getAll'],
		saveLoading: state.loading.effects['crud/create'] || state.loading.effects['crud/update']
	};
})(CRUD);

export default CRUD;
