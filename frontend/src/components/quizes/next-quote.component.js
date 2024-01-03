import React from 'react';
import { useNavigate } from 'react-router-dom';

const NextQuote = ({ quote, getNextQuote, retryQuiz }) => {
    const navigate = useNavigate();

    const quizList = () => {
        navigate('/quiz-list/');
    };

    return (
        quote.isLast ? 
        <div>
            <button className="btn btn-primary mb-2" type="button" onClick={() => quizList()}>Quiz list</button>  
            &nbsp;  &nbsp; 
            <button className="btn btn-primary mb-2" type="button" onClick={() => retryQuiz(quote.quizId)}>Retry quiz</button>
        </div>
        : <button className="btn btn-primary mb-2" type="button" onClick={() => getNextQuote(quote.quizId)}>Get next quote</button>
    );
}

export default NextQuote;