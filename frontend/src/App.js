import React from 'react';
import { Route, Routes, Link, HashRouter } from 'react-router-dom';

import LoginForm from './components/auth/login-form.component';
import CreateUserForm from './components/auth/create-user-form.component';
import { useAuth } from './components/auth/auth.context';

import UserManagement from './components/user-management/user-management.component';
import QuizEditList from './components/quiz-management/quiz-edit-list.component';
import QuizEditForm from './components/quiz-management/quiz-edit-form.component';
import UserReviewList from './components/user-review/user-review-list.component';

import QuizList from './components/quizes/quiz-list.component';
import QuizStart from './components/quizes/quiz-start.component';

function App() {
    const { token, logout, username, getRole } = useAuth();

    // NOT AUTHORIZED
    if (!token) {
        return (
            <HashRouter>
                <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
                    <a className="navbar-brand" href="/">Quote Quiz APP</a>
                    <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="collapse navbar-collapse" id="navbarNav">
                        <ul className="navbar-nav ml-auto">
                            <li className="nav-item">
                                <Link to="/create-user" className="nav-link">Create User</Link>
                            </li>
                            <li className="nav-item">
                                <Link to="/" className="nav-link">Login</Link>
                            </li>
                        </ul>
                    </div>
                </nav>
                <Routes>
                    <Route path="/" element={<LoginForm />}></Route>
                    <Route path="/login" element={<LoginForm />}></Route>
                    <Route path="/create-user" element={<CreateUserForm />}></Route>
                </Routes>
            </HashRouter>);
    }

    // AUTHORIZED
    return (
        <HashRouter>
            <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
                <span className="navbar-brand">Welcome {username()}</span>
                <button className="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="collapse navbar-collapse" id="navbarNav">
                    <ul className="navbar-nav ml-auto">
                        {getRole().isAdmin ?
                            <>
                                <li className="nav-item">
                                    <Link to="/user-management" className="nav-link">User Management</Link>
                                </li>
                                <li className="nav-item">
                                    <Link to="/quiz-edit-list" className="nav-link">Quiz Management</Link>
                                </li>
                            </>
                            : <></>}
                        <li className="nav-item">
                            <Link to="/user-review-list" className="nav-link">User Review</Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/quiz-list" className="nav-link">Quiz List</Link>
                        </li>
                        <li className="nav-item">
                            <Link to="/" onClick={logout} className="nav-link">Logout</Link>
                        </li>
                    </ul>
                </div>
            </nav>
            <Routes>
                <Route path="/" element={getRole().isAdmin ? <UserManagement /> : <QuizList/>}></Route>
                <Route path="/user-management" element={<UserManagement />}></Route>
                <Route path="/quiz-edit-list" element={<QuizEditList />}></Route>
                <Route path="/quiz-edit-form/:quizId" element={<QuizEditForm />}></Route>
                <Route path="/quiz-list" element={<QuizList />}></Route>
                <Route path="/quiz-start/:quizId/:resume" element={<QuizStart />}></Route>
                <Route path="/user-review-list" element={<UserReviewList />}></Route>
            </Routes>
        </HashRouter>);
}


export default App;