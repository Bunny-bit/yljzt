import React from 'react';
import {Modal, Button, Form, Input, Table} from 'antd';
const create = Form.create;
import {connect} from 'dva';


function Chatsearch({dispatch, openSearchFriend, searchfriends, addselect,friendPagination}) {
  function handleOkSearchFriend() {
    console.log(addselect)
    let temp = addselect.map(ele => {
      return {
        tenantId: ele.tenantId,
        userId: ele.id,
      }
    });
    dispatch({
      type: 'home/anyaddfriend',
      payload: temp
    })
  }

  function handleCancel8() {
    dispatch({
      type: 'home/setState',
      payload: {
        openSearchFriend: !openSearchFriend
      }
    });
  }

  function addfriend(text, record) {
    console.log(record)
  }

  function handleTableChange(pagination, filters, sorter) {
    dispatch({
      type: "home/searchfriend",
      payload: {
        skipCount: pagination.pageSize * (pagination.current - 1),
        maxResultCount: pagination.pageSize,
        filter: friendPagination.filter
      }
    });
  }

  const columns = [{
    title: '姓名',
    dataIndex: 'name',
    render: text => <a href="#">{text}</a>,
  }, {
    title: '用户名',
    dataIndex: 'userName',
  }, {
    title: '联系方式',
    dataIndex: 'phoneNumber',
  }, {
    title: '操作', dataIndex: '', key: 'x', render: (text, record) => <Button type="primary" onClick={() => {
      console.log(record)
      dispatch({
        type: 'home/addfriend',
        payload: {
          userId: record.id,
          tenantId: record.tenantId
        }
      })
    }}>添加</Button>
  }];
  const rowSelection = {
    onChange: (selectedRowKeys, selectedRows) => {
      dispatch({
        type: 'home/setState',
        payload: {
          addselect: selectedRows
        }
      })
      console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);

    },
    getCheckboxProps: record => ({
      disabled: record.name === 'Disabled User',
    }),
  };
  return (
    <Modal
      title="好友查找"
      visible={openSearchFriend}
      onOk={handleOkSearchFriend}
      width={600}
      onCancel={handleCancel8}
    >
      <Table 
        rowSelection={rowSelection} 
        columns={columns} 
        dataSource={searchfriends.items}
        pagination={friendPagination}
        onChange={handleTableChange}/>
    </Modal>
  );
}

Chatsearch = connect((state) => {
  return {
    ...state.home,
  }
})(Chatsearch);

export default create()(Chatsearch);
