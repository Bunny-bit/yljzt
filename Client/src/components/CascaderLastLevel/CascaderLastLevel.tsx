import React from 'react';

import { Cascader } from 'antd';

class CascaderLastLevel extends React.Component {
	render() {
		function eachValue(options, value) {
			for (let o of options) {
				if (o.children) {
					let result = eachValue(o.children, value);
					if (result) {
						return [ o.value, ...result ];
					}
				} else {
					if (o.value == value) {
						return [ o.value ];
					}
				}
			}
		}
		let value = eachValue(this.props.options, this.props.value);
		return (
			<Cascader
				style={{ width: '100%' }}
				{...this.props}
				value={value}
				onChange={(value) => this.props.onChange(value[value.length - 1])}
			/>
		);
	}
}

export default CascaderLastLevel;
