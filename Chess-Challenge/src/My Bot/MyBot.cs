using ChessChallenge.API;
using System;
using System.Collections.Generic;

public class MyBot : IChessBot
{
    int[] pieceValues = { 0, 100, 300, 300, 500, 900, 10000 };

    struct BoardControl
    {
        Square square;
        float value;
    }

    public Move Think(Board board, Timer timer)
    {
        Move[] allMoves = board.GetLegalMoves();

        // Pick a random move to play if nothing better is found
        Random rng = new();
        Move moveToPlay = allMoves[rng.Next(allMoves.Length)];
        int highestValueCapture = 0;

        foreach (Move move in allMoves)
        {
            // Always play checkmate in one
            if (MoveIsCheckmate(board, move))
            {
                moveToPlay = move;
                break;
            }

            // Find highest value capture
            Piece capturedPiece = board.GetPiece(move.TargetSquare);
            int capturedPieceValue = pieceValues[(int)capturedPiece.PieceType];
            int AvailableMoves = board.GetLegalMoves().Length;
            board.MakeMove(move);
            int opponentAvailableMoves = board.GetLegalMoves().Length;

            if (board.TrySkipTurn())
            {
                AvailableMoves = board.GetLegalMoves().Length;
                board.UndoSkipTurn();
            }

            capturedPieceValue += AvailableMoves - opponentAvailableMoves;

            board.UndoMove(move);
            if (board.SquareIsAttackedByOpponent(move.TargetSquare))
            {
                capturedPieceValue -= pieceValues[(int)move.MovePieceType];
            }


            if (capturedPieceValue > highestValueCapture)
            {
                moveToPlay = move;
                highestValueCapture = capturedPieceValue;
            }
        }

        return moveToPlay;
    }

    // Test if this move gives checkmate
    bool MoveIsCheckmate(Board board, Move move)
    {
        board.MakeMove(move);
        bool isMate = board.IsInCheckmate();
        board.UndoMove(move);
        return isMate;
    }

    List<BoardControl> EvaluateBoardControl(Board board)
    {
        List<BoardControl> result = new List<BoardControl>();
        return result;
    }
}