// -----------------------------------------------------------------------
// <copyright file="PartOfSpeechType.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    /// <summary>
    /// Descibe part of speecg type.
    /// </summary>
    public enum PartOfSpeechType
    {
        /// <summary>
        /// A noun is the name of a person, place, thing, or idea.
        /// </summary>
        Noun = 0,

        /// <summary>
        /// A pronoun is a word used in place of a noun.
        /// </summary>
        Pronoun = 1,

        /// <summary>
        /// A preposition is a word placed before a noun
        /// or pronoun to form a phrase modifying another
        /// word in the sentence.
        /// </summary>
        Preposition = 2,

        /// <summary>
        /// An adjective modifies or describes a noun or pronoun.
        /// </summary>
        Adjective = 3,

        /// <summary>
        /// An adverb modifies or describes a verb, an adjective, or another adverb.
        /// </summary>
        Adverb = 4,

        /// <summary>
        /// A conjunction joins words, phrases, or clauses.
        /// </summary>
        Conjuction = 5,

        /// <summary>
        /// An interjection is a word used to express emotion.
        /// </summary>
        Interjection = 6,

        /// <summary>
        /// A transitive verb is a verb that accepts one or more objects.
        /// </summary>
        TransitiveVerb = 7,

        /// <summary>
        /// An intransitive verb does not allow a direct object.
        /// </summary>
        IntransitiveVerb = 8,
    }
}
