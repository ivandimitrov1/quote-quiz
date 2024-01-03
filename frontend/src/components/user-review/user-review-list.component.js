import React, { useState, useEffect } from 'react';
import { useAuth } from '../auth/auth.context';

const UserReviewList = () => {
  const { token } = useAuth();

  const [userReviews, setUserReviews] = useState([]);

  const fetchUserReviews = async () => {
    try {
      const response = await fetch(`https://localhost:7197/api/userreview/list`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',
      });

      const data = await response.json();
      setUserReviews(data);
    } catch (error) {
      console.error('Error fetching users:', error);
    }
  };


  useEffect(() => {
    fetchUserReviews();
  }, []);

  return (
    <div className="container mt-4">
      <h2 className="mb-4">User Review</h2>
      <table className="table table-hover">
        <thead className="thead-dark">
          <tr>
            <th> ID </th>
            <th> Login </th>
            <th> Quiz</th>
            <th> Started on</th>
            <th> Correct answers </th>
          </tr>
        </thead>
        <tbody>
          {userReviews.map(user => (
            <tr key={user.userId}>
              <td>{user.userId}</td>
              <td>{user.login}</td>
              <td>{user.quizTitle}</td>
              <td>{user.startedOn}</td>
              <td>{user.correctAnswers} / {user.quoteCount}</td>
            </tr>
          ))}
        </tbody>
      </table>
      <div>
      </div>
    </div>
  );
};

export default UserReviewList;