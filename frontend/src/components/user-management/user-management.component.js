import React, { useState, useEffect } from 'react';
import { useAuth } from '../auth/auth.context';
import e from 'cors';

const UserManagement = () => {
  const { token } = useAuth();

  const [users, setUsers] = useState([]);
  const [orderBy, setOrderBy] = useState('id');
  const [orderDirection, setOrderDirection] = useState('true');

  const [searchByName, setSearchByName] = useState("");
  const [searchByLogin, setSearchByLogin] = useState("");

  const [editUser, setEditUser] = useState({
    id: 0,
    name: '',
    role: {
      id: 0,
      value: ''
    },
  });

  const [feedback, setFeedback] = useState(null);
  const successStyle = "alert alert-success";
  const errorStyle = "alert alert-danger";

  const fetchUsers = async (selectedOrderBy, isAsc) => {
    try {
      const response = await fetch(`https://localhost:7197/api/usermanagement/list?column=${(selectedOrderBy || orderBy)}&Asc=${(isAsc || orderDirection)}&name=${searchByName}&login=${searchByLogin}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',
      });

      const data = await response.json();
      setUsers(data);
    } catch (error) {
      setFeedback({ style: errorStyle, message: "Something went wrong." });
    }
  };

  const handleDisableUser = async (userId) => {
    try {
      const response = await fetch(`https://localhost:7197/api/usermanagement/${userId}/disable`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',
      });

      applyResult(response);
    } catch (error) {
      setFeedback({ style: errorStyle, message: "Something went wrong." });
    }
  };

  const handleDeleteUser = async (userId) => {

    try {
      const response = await fetch(`https://localhost:7197/api/usermanagement/${userId}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',
      });

      applyResult(response);
    } catch (error) {
      setFeedback({ style: errorStyle, message: "Something went wrong." });
    }
  };

  const applyResult = async (response) => {
    if (response.ok) {
      setFeedback({ style: successStyle, message: "Successfull operation." })
      fetchUsers();
    } else {
      var result = await response.json();
      setFeedback({ style: errorStyle, message: result.detail })
    }
  };

  const handleOrderBy = (column) => {
    const toggleAsc = orderDirection === 'true' ? 'false' : 'true';

    setOrderBy(column);
    setOrderDirection(toggleAsc);
    fetchUsers(column, toggleAsc);
  };

  const handleSearch = (e) => {
    e.preventDefault();

    fetchUsers();
  };

  const handleEditUser = (user) => {
    setEditUser({
      id: user.id,
      name: user.name,
      role: user.role
    })
  };

  const handleNameChange = (e) => {
    setEditUser(prevState => ({ ...prevState, name: e.target.value }))
  };

  const handleRoleChange = (e) => {
    setEditUser(prevState => ({ ...prevState, role: { id: e.target.value, value: '' } }));
  };

  const handleApplyUser = async () => {
    try {
      const response = await fetch(`https://localhost:7197/api/usermanagement/${editUser.id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',
        body: JSON.stringify({ id: editUser.id, name: editUser.name, role: { id: editUser.role.id, value: '' } }),
      });

      applyResult(response);
    } catch (error) {
      setFeedback({ style: errorStyle, message: "Something went wrong." });
    }
  };


  const handleCancelEdit = () => {
    setEditUser({
      id: 0,
      name: '',
      role: { id: 0, value: '' },
    })
  };


  const sortButton = (field) => {

    return (
      <button onClick={() => handleOrderBy(field)} className="btn btn-sm btn-light ml-2"
        style={orderBy === field ? { border: '1px solid' } : {}}
      >
        {orderDirection === 'true' ? '↓' : '↑'}
      </button>
    );
  }


  useEffect(() => {
    fetchUsers();
  }, []);

  return (
    <div>

      <div className="container mt-4">
        <h2 className="mb-4">Search</h2>
        <form onSubmit={handleSearch} className="form-inline">
          <div class="input-group">
            <div className="form-group mx-sm-3 mb-2">
              <input
                type="text"
                className="form-control"
                placeholder="Search by name"
                value={searchByName}
                onChange={(e) => setSearchByName(e.target.value)}
              />
            </div>
            <div className="form-group mx-sm-3 mb-2">
              <input
                type="text"
                className="form-control"
                placeholder="Search by login"
                value={searchByLogin}
                onChange={(e) => setSearchByLogin(e.target.value)}
              />
            </div>
            <button type="submit" className="btn btn-primary">Search</button>
          </div>

        </form>
      </div>

      <div>
        <div className="container mt-4">
          <h2 className="mb-4">User Management</h2>
          {feedback &&
            <div class={feedback.style} role="alert">
              {feedback.message}
            </div>
          }
          <table className="table table-hover">
            <thead className="thead-dark">
              <tr>
                <th>Id {sortButton('id')}</th>
                <th>Name {sortButton('name')}</th>
                <th>Role {sortButton('role')}</th>
                <th>Login {sortButton('login')}</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {users.map(user => (
                user.id === editUser.id ?
                  <tr key={user.id}>
                    <td>{user.id}</td>
                    <td><input type="text" id="editUserName" value={editUser.name} onChange={(e) => handleNameChange(e)} /></td>
                    <td>
                      <select id="roleDropdown" value={editUser.role.id} onChange={(e) => handleRoleChange(e)}>
                        <option value={1}>User</option>
                        <option value={2}>Admin</option>
                        <option value={3}>UserReadonly</option>
                      </select>
                    </td>
                    <td>{user.login}</td>
                    <td>
                      <button onClick={() => handleApplyUser()}>Apply</button>
                      <button onClick={() => handleCancelEdit(user)}>Cancel</button>
                    </td>
                  </tr>
                  :
                  <tr key={user.id}>
                    <td>{user.id}</td>
                    <td>{user.name}</td>
                    <td>{user.role.value}</td>
                    <td>{user.login}</td>
                    <td>
                      <button onClick={() => handleEditUser(user)}>Edit</button>
                      <button onClick={() => handleDisableUser(user.id)}>Disable</button>
                      <button onClick={() => handleDeleteUser(user.id)}>Delete</button>
                    </td>
                  </tr>
              ))}
            </tbody>
          </table>
        </div>
        <div>
        </div>
      </div>
    </div>
  );
};

export default UserManagement;