# Changelog
All notable changes to this package will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).


## [2.2.8](https://github.com/CareBoo/Serially/compare/v2.2.7...v2.2.8) (2022-12-30)


### Bug Fixes

* **context menu:** move managed reference context menu items to the default context menu ([841b3a0](https://github.com/CareBoo/Serially/commit/841b3a011c67f9665fe1b64b80d72cf1ff98fd22)), closes [#75](https://github.com/CareBoo/Serially/issues/75)
* **show serialize reference:** filter objects derived from `UnityEngine.Object` from possible values ([0fe75d2](https://github.com/CareBoo/Serially/commit/0fe75d21668ffae8224e26919872010d4c539677)), closes [#74](https://github.com/CareBoo/Serially/issues/74)

## [2.2.7](https://github.com/CareBoo/Serially/compare/v2.2.6...v2.2.7) (2022-06-22)


### Bug Fixes

* **gui:** fix flex and ordering in type window ([1608348](https://github.com/CareBoo/Serially/commit/1608348d5b11672da8ae4f777f00e0f889b3308a))

## [2.2.6](https://github.com/CareBoo/Serially/compare/v2.2.5...v2.2.6) (2022-05-20)


### Bug Fixes

* **license:** add LICENSE.md.meta file to package ([#72](https://github.com/CareBoo/Serially/issues/72)) ([49cc25a](https://github.com/CareBoo/Serially/commit/49cc25ac3563a25ff157f10e0eb9e707a7541407)), closes [#71](https://github.com/CareBoo/Serially/issues/71)

## [2.2.5](https://github.com/CareBoo/Serially/compare/v2.2.4...v2.2.5) (2022-04-23)


### Bug Fixes

* fix compile err in 2021.1 ([#70](https://github.com/CareBoo/Serially/issues/70)) ([cf51cc3](https://github.com/CareBoo/Serially/commit/cf51cc37e48b5270e95ee80959916a0ab11b2683)), closes [#69](https://github.com/CareBoo/Serially/issues/69)

## [2.2.4](https://github.com/CareBoo/Serially/compare/v2.2.3...v2.2.4) (2022-04-10)


### Bug Fixes

* **editor:** fix getting field value on a property ([3834970](https://github.com/CareBoo/Serially/commit/38349702a6630b3cf6df6a9bae4b506797269719))
* use new API for Unity version 2021+ ([4623df1](https://github.com/CareBoo/Serially/commit/4623df1d8c2720cac77cefd1b41b31dbd6b9654e))

## [2.2.4-pre.1](https://github.com/CareBoo/Serially/compare/v2.2.3...v2.2.4-pre.1) (2022-04-10)


### Bug Fixes

* **editor:** fix getting field value on a property ([3834970](https://github.com/CareBoo/Serially/commit/38349702a6630b3cf6df6a9bae4b506797269719))
* use new API for Unity version 2021+ ([4623df1](https://github.com/CareBoo/Serially/commit/4623df1d8c2720cac77cefd1b41b31dbd6b9654e))

## [2.2.3](https://github.com/CareBoo/Serially/compare/v2.2.2...v2.2.3) (2020-10-23)


### Bug Fixes

* :bug: Fix icons not showing ([b777b78](https://github.com/CareBoo/Serially/commit/b777b78ddb3eea1e7dd2032ebefda987c4da9760))
* :bug: Fix null reference if no TypeFilter specified ([a9d1edb](https://github.com/CareBoo/Serially/commit/a9d1edbe124dd4279d17034d5e88cf7b42742a21)), closes [#63](https://github.com/CareBoo/Serially/issues/63)
* :bug: Fix warning when clicking null serialize reference ([8fec535](https://github.com/CareBoo/Serially/commit/8fec53523310162f53c2d2c83689bb5df68d3288))

## [2.2.2](https://github.com/CareBoo/Serially/compare/v2.2.1...v2.2.2) (2020-08-17)


### Bug Fixes

* :bug: typefilter returning null wasn't properly handled ([43928f9](https://github.com/CareBoo/Serially/commit/43928f9577c86a1264533f135f60ad3c13c8dcd8)), closes [#61](https://github.com/CareBoo/Serially/issues/61)

## [2.2.1](https://github.com/CareBoo/Serially/compare/v2.2.0...v2.2.1) (2020-08-16)


### Bug Fixes

* :ambulance: fixes renamed GuiEvent meta file ([53f665b](https://github.com/CareBoo/Serially/commit/53f665b0108858b3e0bb93f3198c4e66fa90beb7)), closes [#56](https://github.com/CareBoo/Serially/issues/56)

# [2.2.0](https://github.com/CareBoo/Serially/compare/v2.1.0...v2.2.0) (2020-08-16)


### Features

* :sparkles: add TypeFilterAttribute support ([aafd1a1](https://github.com/CareBoo/Serially/commit/aafd1a10b69f2275cbeb8e77cb9b7ff977ce727b)), closes [#52](https://github.com/CareBoo/Serially/issues/52)
* :sparkles: modify TypeFilterAttribute to use sequences ([12558d5](https://github.com/CareBoo/Serially/commit/12558d5b9c7ac98c00131900908bf321a33f78cd)), closes [#54](https://github.com/CareBoo/Serially/issues/54)


### Performance Improvements

* :zap: making selectabletypes lazy ([4e3b2fc](https://github.com/CareBoo/Serially/commit/4e3b2fc10438d17d434cd3acd90541b892613ac3)), closes [#53](https://github.com/CareBoo/Serially/issues/53)

# [2.2.0-preview.1](https://github.com/CareBoo/Serially/compare/v2.1.0...v2.2.0-preview.1) (2020-08-16)


### Features

* :sparkles: add TypeFilterAttribute support ([23f32c6](https://github.com/CareBoo/Serially/commit/23f32c6cbf94c25763e008402c9c6928f737bbd4)), closes [#52](https://github.com/CareBoo/Serially/issues/52)
* :sparkles: modify TypeFilterAttribute to use sequences ([e491674](https://github.com/CareBoo/Serially/commit/e491674b352b5821661163034ef32f03acbf24cd)), closes [#54](https://github.com/CareBoo/Serially/issues/54)


### Performance Improvements

* :zap: making selectabletypes lazy ([a440a52](https://github.com/CareBoo/Serially/commit/a440a52964a55fcd5fd0b9dd58fc1a90537d2343)), closes [#53](https://github.com/CareBoo/Serially/issues/53)

# [2.1.0-preview.3](https://github.com/CareBoo/Serially/compare/v2.1.0-preview.2...v2.1.0-preview.3) (2020-08-16)


### Features

* :sparkles: modify TypeFilterAttribute to use sequences ([71b0ae5](https://github.com/CareBoo/Serially/commit/71b0ae5b9e58631672895dc5a138b191bfcd9055)), closes [#54](https://github.com/CareBoo/Serially/issues/54)

# [2.1.0-preview.2](https://github.com/CareBoo/Serially/compare/v2.1.0-preview.1...v2.1.0-preview.2) (2020-08-16)


### Performance Improvements

* :zap: making selectabletypes lazy ([fd4d28a](https://github.com/CareBoo/Serially/commit/fd4d28ab623ff11cdc3507de0bc8156eb6496413)), closes [#53](https://github.com/CareBoo/Serially/issues/53)

# [2.1.0-preview.1](https://github.com/CareBoo/Serially/compare/v2.0.0...v2.1.0-preview.1) (2020-08-16)


### Bug Fixes

* :art: fixing background color regardless of editor skin ([a871caf](https://github.com/CareBoo/Serially/commit/a871caf71cc5b6fb0852d2999f3616046af7ff15))
* :art: fixing background color regardless of editor skin ([87bb2a1](https://github.com/CareBoo/Serially/commit/87bb2a1c1436636f9a289da5ab43a15ea24a6c34))
* :bug: fixing bug if absolute file path is windows ([142b729](https://github.com/CareBoo/Serially/commit/142b729ccf4b70fd627dd436e128d1b9b73ef4f5))
* :bug: fixing bug if absolute file path is windows ([fb1eafc](https://github.com/CareBoo/Serially/commit/fb1eafc6000dc31b75fe998f0566ed0d9a287fb6))


### Features

* :sparkles: copy paste SerializeReference fields! ([559a010](https://github.com/CareBoo/Serially/commit/559a01092dda0bf65a6e5a02b39dd3489f327ee4))

# [2.0.0](https://github.com/CareBoo/Serially/compare/v1.0.2...v2.0.0) (2020-08-16)


### Code Refactoring

* :art: refactoring typefield ([d574868](https://github.com/CareBoo/Serially/commit/d574868c210cde549f70c3051ef6a68fe332e03b))


### BREAKING CHANGES

* Changes the public API of TypeField and remove TypeFieldOptions

## [1.0.2](https://github.com/CareBoo/Serially/compare/v1.0.1...v1.0.2) (2020-08-15)


### Bug Fixes

* :bug: fixing can't set null type on SerializableType ([afdb027](https://github.com/CareBoo/Serially/commit/afdb0279c2a60fd1934ce00ab155a77fefe2a24c)), closes [#46](https://github.com/CareBoo/Serially/issues/46)

## [1.0.1](https://github.com/CareBoo/Serially/compare/v1.0.0...v1.0.1) (2020-08-15)


### Bug Fixes

* :bug: fixes error in demo ([47e7470](https://github.com/CareBoo/Serially/commit/47e7470dc4a1d19fe613ebf862474db37b3ba473)), closes [#48](https://github.com/CareBoo/Serially/issues/48)

# [1.0.0](https://github.com/CareBoo/Serially/compare/v0.1.4...v1.0.0) (2020-07-30)


### Code Refactoring

* :art: ShowValueType -> ShowSerializeReference ([0feed58](https://github.com/CareBoo/Serially/commit/0feed58f7dc42e2ff7930f357c058ba2113853fb))


### BREAKING CHANGES

* ShowValueType is now renamed as ShowSerializeReference. Fixes #37

# [1.0.0-preview.1](https://github.com/CareBoo/Serially/compare/v0.1.4...v1.0.0-preview.1) (2020-07-29)


### Code Refactoring

* :art: ShowValueType -> ShowSerializeReference ([0feed58](https://github.com/CareBoo/Serially/commit/0feed58f7dc42e2ff7930f357c058ba2113853fb))


### BREAKING CHANGES

* ShowValueType is now renamed as ShowSerializeReference. Fixes #37
