import React from 'react';
import { Tree, Input } from 'antd';
import { connect } from 'dva';

const TreeNode = Tree.TreeNode;
const Search = Input.Search;

function SearchTree({ dispatch, organizations, selectedOrganizations, organizationFilter }) {
	function filterOrganizations() {
		if (!organizationFilter || !organizationFilter.length) {
			return organizations;
		}
		return loopFilterOrganizations(organizations);
	}

	function loopFilterOrganizations(data) {
		var result = [];
		data.map((item) => {
			var node = { ...item };
			if (node.children && node.children.length) {
				node.children = loopFilterOrganizations(node.children);
			}
			if (node.displayName.indexOf(organizationFilter) >= 0 || (node.children && node.children.length)) {
				result.push(node);
			}
		});
		return result;
	}

	const loop = (data) =>
		data.map((item) => {
			if (item.children) {
				return (
					<TreeNode key={item.id} title={item.displayName}>
						{loop(item.children)}
					</TreeNode>
				);
			}
			return <TreeNode key={item.id} title={item.displayName} />;
		});

	function onSearch(e) {
		dispatch({
			type: 'user/setState',
			payload: {
				organizationFilter: e.target.value
			}
		});
	}

	return (
		<div>
			<Search placeholder="输入关键字搜索" onChange={onSearch} />
			<Tree
				checkable
				checkedKeys={selectedOrganizations}
				checkStrictly={true}
				defaultExpandAll={true}
				onCheck={(selectedKeys, e) => {
					dispatch({
						type: 'user/setState',
						payload: {
							selectedOrganizations: selectedKeys.checked
						}
					});
				}}
			>
				{loop(filterOrganizations())}
			</Tree>
		</div>
	);
}
export default connect((state) => {
	return {
		...state.user
	};
})(SearchTree);
