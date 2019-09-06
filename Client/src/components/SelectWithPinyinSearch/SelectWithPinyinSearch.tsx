import React from 'react';
import 'pinyin4js';

import { Select } from 'antd';
const Option = Select.Option;

class SelectWithPinyinSearch extends React.Component {
	render() {
		const {
			value,
			defaultValue,
			onChange,
			placeholder,
			size = 'default',
			allowClear = false,
			disabled = false
		} = this.props;
		const children = [];
		if (this.props.options && this.props.options.length) {
			for (let option of this.props.options) {
				let spellCode = PinyinHelper.convertToPinyinString(option.name, '', PinyinFormat.FIRST_LETTER);
				let _pinyin = PinyinHelper.convertToPinyinString(option.name, '', PinyinFormat.WITHOUT_TONE);

				children.push(
					<Option
						key={option.value}
						spellCode={spellCode.toString().replace(/,/g, '')}
						pinyin={_pinyin.toString().replace(/,/g, '')}
					>
						{option.name}
					</Option>
				);
			}
		}
		return (
			<Select
				onChange={(value) => onChange(value)}
				defaultValue={defaultValue}
				value={value}
				showSearch
				allowClear={allowClear}
				disabled={disabled}
				style={{ width: '100%' }}
				placeholder={placeholder}
				optionFilterProp="children"
				filterOption={(input, option) =>
					option.props.children.toLowerCase().indexOf(input.toLowerCase()) >= 0 ||
					option.props.spellCode.toLowerCase().indexOf(input.toLowerCase()) >= 0 ||
					option.props.pinyin.toLowerCase().indexOf(input.toLowerCase()) >= 0}
			>
				{children}
			</Select>
		);
	}
}

export default SelectWithPinyinSearch;
