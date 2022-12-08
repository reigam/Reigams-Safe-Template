module Database

open LiteDB.FSharp
open LiteDB

let database dbName =
    let mapper = FSharpBsonMapper()
    let dbFile = $"{dbName}.db"
    let connStr = $"Filename={dbFile};mode=Exclusive"
    new LiteDatabase( connStr, mapper )


