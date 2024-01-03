import React, { useState, useEffect } from 'react';
import NextQuote from './next-quote.component';

const YesNoQuote = ({ quote, answerQuote, getNextQuote, correctAnswer, retryQuiz }) => {

    return (
        <div className="card-body">
            <blockquote className="blockquote text-center">
                <p className="mb-0">{quote.quizTitle}</p>
                <footer className="blockquote-footer mt-2">{quote.quoteText}</footer>
            </blockquote>
            <div className="text-center">
                {correctAnswer ?
                    <div>
                        {
                            correctAnswer.isCorrect ?
                                <div class="alert alert-success" role="alert">Correct</div>
                                : <div class="alert alert-danger" role="alert">Incorrect</div>
                        }

                        <NextQuote quote={quote} getNextQuote={getNextQuote} retryQuiz={retryQuiz} />
                    </div>
                    : <div>
                        {quote.quoteAnswers.map(answer => (
                            <div><h3>{answer}</h3></div>
                        ))}

                        <button
                            className="btn btn-success mr-2"
                            onClick={() => answerQuote(quote.quizId, quote.quoteId, 1)}>
                            Yes
                        </button>
                        &nbsp;&nbsp;
                        <button
                            className="btn btn-danger"
                            onClick={() => answerQuote(quote.quizId, quote.quoteId, 0)}>
                            No
                        </button>
                    </div>
                }
            </div>
        </div>
    );
}

export default YesNoQuote;