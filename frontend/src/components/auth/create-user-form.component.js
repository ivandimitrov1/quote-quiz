import React, { useState } from 'react';

const CreateUserForm = () => {
  const [formData, setFormData] = useState({
    name: '',
    login: '',
    password: '',
    confirmPassword: '',
  });
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);

  const handleChange = (e) => {
    const { id, value } = e.target;
    setFormData((prevData) => ({
      ...prevData,
      [id]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const { name, login, password, confirmPassword } = formData;
      const response = await fetch('https://localhost:7197/api/auth/create', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ name, login, password, confirmPassword }),
      });

      if (!response.ok) {
        const result = await response.json();
        setError(result.detail || JSON.stringify(result.errors));
        setSuccess('');
      } else {
        setError('');
        setSuccess("User was created successfully. Go to login page.");
      }

    } catch (error) {
      setError("Someting went wrong.");
    }
  }

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-md-6">
          <div className="card">
            <div className="card-body">
              <form onSubmit={handleSubmit}>
                <h3 className="text-center">Create user</h3>

                {error &&
                  <div class="alert alert-danger" role="alert">
                    {error}
                  </div>
                }
                {success &&
                  <div class="alert alert-success" role="alert">
                    {success}
                  </div>
                }

                <div className="form-group">
                  <div class="input-group mb-3">
                    <div class="input-group-prepend col-sm-3">
                      <span class="input-group-text" id="inputGroup-sizing-default">Login</span>
                    </div>
                    <input id="login" type="text" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default"
                      placeholder="Enter login" value={formData.login} onChange={handleChange} />
                  </div>
                </div>

                <div className="form-group">
                  <div class="input-group mb-3">
                    <div class="input-group-prepend col-sm-3">
                      <span class="input-group-text" id="inputGroup-sizing-default">Name</span>
                    </div>
                    <input id="name" type="text" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default"
                      placeholder="Enter name" value={formData.name} onChange={handleChange} />
                  </div>
                </div>

                <div className="form-group">
                  <div class="input-group mb-3">
                    <div class="input-group-prepend col-sm-3">
                      <span class="input-group-text" id="inputGroup-sizing-default">Password</span>
                    </div>
                    <input id="password" type="password" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default"
                      placeholder="Enter password" value={formData.password} onChange={handleChange} />
                  </div>
                </div>

                <div className="form-group">
                  <div class="input-group mb-3">
                    <div class="input-group-prepend col-sm-3">
                      <span class="input-group-text" id="inputGroup-sizing-default">Confirm password</span>
                    </div>
                    <input id="confirmPassword" type="password" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default"
                      placeholder="Enter the same password" value={formData.confirmPassword} onChange={handleChange} />
                  </div>
                </div>

                <button type="submit" className="btn btn-primary btn-block">Create user</button>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CreateUserForm;