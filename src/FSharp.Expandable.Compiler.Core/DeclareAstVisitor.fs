﻿//////////////////////////////////////////////////////////////////////////////
// 
// fscx - Expandable F# compiler project
//   Author: Kouji Matsui (@kekyo2), bleis-tift (@bleis-tift)
//   GutHub: https://github.com/fscx-projects/
//
// Creative Commons Legal Code
// 
// CC0 1.0 Universal
// 
//   CREATIVE COMMONS CORPORATION IS NOT A LAW FIRM AND DOES NOT PROVIDE
//   LEGAL SERVICES.DISTRIBUTION OF THIS DOCUMENT DOES NOT CREATE AN
//   ATTORNEY-CLIENT RELATIONSHIP.CREATIVE COMMONS PROVIDES THIS
//   INFORMATION ON AN "AS-IS" BASIS.CREATIVE COMMONS MAKES NO WARRANTIES
//   REGARDING THE USE OF THIS DOCUMENT OR THE INFORMATION OR WORKS
//   PROVIDED HEREUNDER, AND DISCLAIMS LIABILITY FOR DAMAGES RESULTING FROM
//   THE USE OF THIS DOCUMENT OR THE INFORMATION OR WORKS PROVIDED
//   HEREUNDER.
//
//////////////////////////////////////////////////////////////////////////////

namespace Microsoft.FSharp.Compiler.Ast.Visitors

open System
open Microsoft.FSharp.Compiler.Ast
open Microsoft.FSharp.Compiler.Ast.Visitors
open Microsoft.FSharp.Compiler.SourceCodeServices

#nowarn "44"

//////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Target interface for fscx filter.
/// </summary>
type IDeclareAstVisitor =

  /// <summary>
  /// Visit the parsed input (Entry point).
  /// </summary>
  /// <param name="symbolInformation">Symbol information.</param>
  /// <param name="parsedInput">Target for ParsedInput instance.</param>
  /// <returns>Visited instance.</returns>
  abstract Visit :
    symbolInformation: Microsoft.FSharp.Compiler.SourceCodeServices.FSharpCheckFileResults * parsedInput: Microsoft.FSharp.Compiler.Ast.ParsedInput ->
    Microsoft.FSharp.Compiler.Ast.ParsedInput
    
//////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Basic functional visitor base type.
/// </summary>
/// <typeparam name="'TVisitor">Custom visitor type.</typeparam>
/// <typeparam name="'TContext">Custom context type.</typeparam>
/// <remarks>
/// Inherit this class if use AstFunctionalVisitor.
/// </remarks>
[<AbstractClass; NoEquality; NoComparison; AutoSerializable(false)>]
type DeclareAstInheritableVisitor<'TVisitor,'TContext when 'TVisitor : (new: unit -> 'TVisitor) and 'TVisitor :> AstInheritableVisitor<'TContext> and 'TContext : (new: unit -> 'TContext)>() =

  /// <summary>
  /// Visit the parsed input (Entry point).
  /// </summary>
  /// <param name="symbolInformation">Symbol information.</param>
  /// <param name="parsedInput">Target for ParsedInput instance.</param>
  /// <returns>Visited instance.</returns>
  member __.Visit(symbolInformation, parsedInput) = 
    let visitor = new 'TVisitor()
    visitor.Visit(symbolInformation, parsedInput)

  interface IDeclareAstVisitor with
    /// <summary>
    /// Visit the parsed input (Entry point).
    /// </summary>
    /// <param name="symbolInformation">Symbol information.</param>
    /// <param name="parsedInput">Target for ParsedInput instance.</param>
    /// <returns>Visited instance.</returns>
    member this.Visit(symbolInformation, parsedInput) = 
      this.Visit(symbolInformation, parsedInput)

/// <summary>
/// Basic functional visitor base type.
/// </summary>
/// <remarks>
/// Inherit this class if use AstFunctionalVisitor.
/// </remarks>
[<AbstractClass; NoEquality; NoComparison; AutoSerializable(false)>]
type DeclareAstInheritableVisitor<'TVisitor when 'TVisitor : (new: unit -> 'TVisitor) and 'TVisitor :> AstInheritableVisitor<NoContext>>() =
  inherit DeclareAstInheritableVisitor<'TVisitor, NoContext>()
  
//////////////////////////////////////////////////////////////////////////////

/// <summary>
/// Basic functional visitor base type.
/// </summary>
/// <typeparam name="'TContext">Custom context type.</typeparam>
/// <remarks>
/// Inherit this class if use AstFunctionalVisitor.
/// </remarks>
[<AbstractClass; NoEquality; NoComparison; AutoSerializable(false)>]
type DeclareAstFunctionalVisitor<'TContext when 'TContext: (new: unit -> 'TContext)>
    (visitor: ( (* default: *) FSharpCheckFileResults * 'TContext * SynExpr -> SynExpr) * (* symbolInformation: *) FSharpCheckFileResults * (* context: *) 'TContext * (* target: *) SynExpr -> SynExpr option) =

  /// <summary>
  /// Visit the parsed input (Entry point).
  /// </summary>
  /// <param name="symbolInformation">Symbol information.</param>
  /// <param name="parsedInput">Target for ParsedInput instance.</param>
  /// <returns>Visited instance.</returns>
  member __.Visit(symbolInformation, parsedInput) = 
    AstFunctionalVisitor.visitInput(visitor, symbolInformation, new 'TContext(), parsedInput)

  interface IDeclareAstVisitor with
    /// <summary>
    /// Visit the parsed input (Entry point).
    /// </summary>
    /// <param name="symbolInformation">Symbol information.</param>
    /// <param name="parsedInput">Target for ParsedInput instance.</param>
    /// <returns>Visited instance.</returns>
    member this.Visit(symbolInformation, parsedInput) = 
      this.Visit(symbolInformation, parsedInput)

/// <summary>
/// Basic functional visitor base type.
/// </summary>
/// <remarks>
/// Inherit this class if use AstFunctionalVisitor.
/// </remarks>
[<AbstractClass; NoEquality; NoComparison; AutoSerializable(false)>]
type DeclareAstFunctionalVisitor
    (visitor: ( (* default: *) FSharpCheckFileResults * NoContext * SynExpr -> SynExpr) * (* symbolInformation: *) FSharpCheckFileResults * (* context: *) NoContext * (* target: *) SynExpr -> SynExpr option) =
  inherit DeclareAstFunctionalVisitor<NoContext>(visitor)
