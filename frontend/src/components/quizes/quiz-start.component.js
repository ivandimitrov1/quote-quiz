import React, { useState, useEffect } from 'react';
import { useAuth } from '../auth/auth.context';
import { useParams } from 'react-router-dom';
import YesNoQuote from './yes-no-quote.component';
import MultipleChoiceQuote from './multiple-choice-quote.component';

const QuizStart = () => {
  const { token } = useAuth();
  const { quizId, resume } = useParams();

  const [feedback, setFeedback] = useState();
  const [quote, setQuote] = useState({ quoteAnswers: [] });
  const [quoteFeedback, setQuoteFeedback] = useState();

  useEffect(() => {
    resume === "resume" ? getNextQuote(quizId) : startQuiz(quizId);
  }, []);

  const startQuiz = async (quizId) => {
    try {
      const response = await fetch('https://localhost:7197/api/quizes/' + quizId + '/start', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',
      });

      const quote = await response.json();
      setQuoteFeedback(null);
      setQuote(quote);
    } catch (error) {
      setFeedback(error);
    }
  };


  const getNextQuote = async (quizId) => {
    try {
      const response = await fetch('https://localhost:7197/api/quizes/' + quizId + '/nextquote', {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',
      });

      if (response.ok) {
        const result = await response.json();
        setQuoteFeedback(null);
        setQuote(result);
      } else {
        var result = await response.json();
        setFeedback(result.detail);
      }
    } catch (error) {
      setFeedback(error);
    }
  };

  const answerQuote = async (quizId, quoteId, answer) => {
    try {
      const response = await fetch('https://localhost:7197/api/quizes/' + quizId + '/quote/' + quoteId + "/answer", {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',
        body: JSON.stringify({ quizId: quizId, quoteId: quoteId, userAnswer: answer }),
      });

      if (response.ok) {
        var { isCorrect, correctAnswer } = await response.json();
        setQuoteFeedback({ isCorrect, correctAnswer });
      } else {
        var result = await response.json();
        setFeedback(result.detail);
      }
    } catch (error) {
      console.error('Error on disabling', error);
    }

  };

  return (
    <div className="container mt-5">
      <h2 className="text-center mb-4">Quiz time :)</h2>
      <div className="card">
        {
        quote.isYesNoQuote ? 
            <YesNoQuote 
                quote={quote} 
                getNextQuote={getNextQuote} 
                answerQuote={answerQuote} 
                correctAnswer={quoteFeedback}
                retryQuiz={startQuiz} /> 
          : <MultipleChoiceQuote 
                quote={quote} 
                getNextQuote={getNextQuote} 
                answerQuote={answerQuote} 
                correctAnswer={quoteFeedback}
                retryQuiz={startQuiz} /> }
      </div>
    </div>
  );
};

export default QuizStart;