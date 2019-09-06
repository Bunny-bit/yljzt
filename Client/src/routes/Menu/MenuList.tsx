import * as React from 'react';
import { Modal, Tree, Button, Input, Icon, Row, Col, message, Dropdown, Menu } from 'antd';
import { connect } from 'dva';
import EditMenuModal from './modal/EditMenuModal';

import styles from './MenuList.css';

const TreeNode = Tree.TreeNode;
const confirm = Modal.confirm;

import PermissionWrapper from './../../components/PermissionWrapper/PermissionWrapper';

//树节点对象
interface node {
	name?: string;
	parentId?: number;
	newParentId?: number;
	items?: Array<node>;
	icon?: string;
	displayName?: string;
}
//无状态组件props对象
interface Props {
	dispatch: (
		parm: {
			type: string;
			payload?: object;
		}
	) => void;
	expand: boolean;
	menus: Array<node>;
	editMenuModalVisible?: boolean;
}

/**
 * Menu.js
 * Created by 凡尧 on 2017/9/12 11:17
 * 描述: 菜单管理界面
 */
function MenuList({ dispatch, expand, menus, editMenuModalVisible }: Props): JSX.Element {
	function onDrop(info: any) {
		// console.log(menus, info);
		const dropKey: string = info.node.props.eventKey;
		const dragKey: string = info.dragNode.props.eventKey;
		const dropPos: string[] = info.node.props.pos.split('-');
		const dropPosition2: number = Number(dropPos[dropPos.length - 1]);
		const dropPosition: number = info.dropPosition + dropPosition2;
		let order: number = info.dropPosition;
		if (info.dropPosition < dropPosition2) {
			order = dropPosition2;
		}
		if (info.dropToGap) {
			var node = findNode(menus, dropKey);
			dispatch({
				type: 'menu/moveMenu',
				payload: {
					id: dragKey,
					newParentId: node.newParentId,
					newOrder: order
				}
			});
		} else {
			dispatch({
				type: 'menu/moveMenu',
				payload: {
					id: dragKey,
					newParentId: dropKey,
					newOrder: order
				}
			});
		}
	}

	function findNode(root: Array<node>, name: string): node {
		for (let i = 0; i < root.length; i++) {
			if (root[i].name == name) {
				let node = {
					newParentId: root[i].parentId
				};
				return node;
			} else {
				if (root[i].items && root[i].items.length > 0) {
					var node = findNode(root[i].items, name);
					if (node) {
						return node;
					}
				}
			}
		}
	}

	const buildTitle: Function = (item: node) => {
		const handlerList = (
			<Menu>
				<Menu.Item>
					<a
						onClick={() => {
							dispatch({
								type: 'menu/setState',
								payload: {
									editMenuModalVisible: !editMenuModalVisible,
									menu: {},
									parentId: item.name,
									isEditingMenu: false,
									selectedIcon: ''
								}
							});
						}}
					>
						添加子级
					</a>
				</Menu.Item>
				<Menu.Item>
					<a
						onClick={() => {
							dispatch({
								type: 'menu/setState',
								payload: {
									editMenuModalVisible: !editMenuModalVisible,
									menu: item,
									parentId: item.parentId,
									isEditingMenu: true,
									selectedIcon: item.icon
								}
							});
						}}
					>
						修改
					</a>
				</Menu.Item>
				<Menu.Item>
					<a onClick={confirmDeleteMenu.bind(this, item)}>删除</a>
				</Menu.Item>
			</Menu>
		);

		return (
			<div>
				<div style={{ float: 'left', marginRight: '30px' }}>
					<Icon type={item.icon} /> {item.displayName}
				</div>
				<PermissionWrapper requiredPermission="Pages.Administration.Menus">
					<Dropdown overlay={handlerList} trigger={[ 'click' ]}>
						<a className="ant-dropdown-link">
							操作
							<Icon type="down" />
						</a>
					</Dropdown>
				</PermissionWrapper>
			</div>
		);
	};

	function confirmDeleteMenu(item: node) {
		confirm({
			title: <div style={{ fontSize: '20px' }}>警告</div>,
			content: <div>{'你确定要删除' + item.displayName + '吗？'} </div>,
			width: 350,
			onOk() {
				dispatch({
					type: 'menu/deleteMenu',
					payload: {
						id: item.name
					}
				});
			},
			onCancel() {}
		});
	}

	const loopMenus: Function = (data: Array<node>) =>
		data.map((item) => {
			if (item.items && item.items.length) {
				return (
					<TreeNode key={item.name} title={buildTitle(item)}>
						{loopMenus(item.items)}
					</TreeNode>
				);
			}
			return <TreeNode key={item.name} title={buildTitle(item)} />;
		});

	return (
		<div>
			<div className={styles.normalT}>
				<PermissionWrapper requiredPermission="Pages.Administration.Menus">
					<Button
						type="primary"
						onClick={() => {
							dispatch({
								type: 'menu/setState',
								payload: {
									editMenuModalVisible: !editMenuModalVisible,
									menu: {},
									parentId: '0',
									isEditingMenu: false,
									selectedIcon: ''
								}
							});
						}}
					>
						添加根菜单
					</Button>
				</PermissionWrapper>
			</div>
			<div className={styles.normalB}>
				<Tree showLine className="draggable-tree" draggable onDrop={onDrop}>
					{loopMenus(menus)}
				</Tree>
			</div>

			{editMenuModalVisible ? <EditMenuModal /> : null}
		</div>
	);
}
interface stateObj {
	menu: object;
}
let DvaMenuList = connect((state: stateObj) => {
	return {
		...state.menu
	};
})(MenuList);

export default DvaMenuList;
